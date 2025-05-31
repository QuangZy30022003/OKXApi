using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Repositories.Enities;
using Repositories.Enities.OKx;
using Repositories.Interfaces;
using Repositories.Models;
using Repositories.Repository;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services.Service
{
    public class OKXService : IOKXService
    {

        private readonly HttpClient _httpClient;
        private readonly IOKXKeyRepository _okxRepository;
        private readonly string _baseUrl = "https://www.okx.com";
        private readonly ISubAccountRepository _subAccountRepo;

        public OKXService(HttpClient httpClient, IConfiguration configuration, IOKXKeyRepository oKXKeyRepository, ISubAccountRepository subAccount)
        {
            _httpClient = httpClient;
            _okxRepository = oKXKeyRepository;
            _subAccountRepo = subAccount;
        }

      

        public async Task<string> GetBalanceAsync()
        {
            var path = "/api/v5/account/balance";
            var request = new HttpRequestMessage(HttpMethod.Get, _baseUrl + path);
            _okxRepository.AddAuthenticationHeaders(request, "GET", path);

            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetTradeHistoryAsync(string instType)
        {
            var path = $"/api/v5/trade/fills-history?instType={instType}";
            var request = new HttpRequestMessage(HttpMethod.Get, _baseUrl + path);
            _okxRepository.AddAuthenticationHeaders(request, "GET", path, "");

            var response = await _httpClient.SendAsync(request);

            var result = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Status: {response.StatusCode}, Body: {result}");
            }

            if (string.IsNullOrWhiteSpace(result) || result.Contains("[]"))
            {
                Console.WriteLine("⚠️ API trả về rỗng hoặc không có giao dịch nào.");
            }
            return result;
        }

        

        public async Task<bool> CreateSubAccountAsync(string subAccountName, string type, string label, string apiKey)
        {
            if (string.IsNullOrWhiteSpace(subAccountName))
                throw new ArgumentException("SubAccountName không được để trống.");

            // Gọi API OKX tạo subaccount
            var rawResult = await _okxRepository.CreateSubAccountAsync(subAccountName, type, label);

            var jsonResult = JsonConvert.DeserializeObject<OkxApiResponse>(rawResult);

            if (jsonResult == null)
                throw new Exception("Không nhận được phản hồi từ OKX.");

            if (jsonResult.Code == "0")
            {
                var okxKey = await _subAccountRepo.GetOkxKeyByApiKeyAsync(apiKey);
                if (okxKey == null)
                    throw new Exception("Không tìm thấy OKXKey tương ứng để lưu SubAccount.");

                var subAccount = new SubAccount
                {
                    SubAccountName = subAccountName,
                    OkxkeyId = okxKey.Id,
                    ParentUserId = okxKey.UserId,
                    CreatedAt = DateTime.UtcNow
                };

                await _subAccountRepo.AddAsync(subAccount);
                return true;
            }
            else
            {
                throw new Exception($"Tạo SubAccount thất bại: {jsonResult.Msg}");
            }
        }

        public async Task<string> GetPriceTrendAsync(string instId)
        {
            var rawJson = await _okxRepository.GetTickerAsync(instId);

            var parsed = JsonConvert.DeserializeObject<OkxTickerResponse>(rawJson);
            if (parsed == null || parsed.Code != "0")
            {
                var msg = parsed?.Msg ?? "Không thể phân tích dữ liệu từ OKX.";
                throw new Exception($"Lỗi từ OKX: {msg}");
            }

            var data = parsed.Data.FirstOrDefault();
            if (data == null)
                throw new Exception("Không có dữ liệu từ OKX.");

            decimal last = decimal.Parse(data.Last);
            decimal open24h = decimal.Parse(data.Open24h);

            if (last > open24h) return "up";
            if (last < open24h) return "down";
            return "neutral";
        }
    }
}
