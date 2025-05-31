using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Repositories.Interfaces;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class OKXKeyRepository : IOKXKeyRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _apiSecret;
        private readonly string _passphrase;
        public OKXKeyRepository(ApplicationDbContext context, HttpClient httpClient, IConfiguration configuration)
        {
            _context = context;
            _httpClient = httpClient;
            _apiKey = configuration["OKX:ApiKey"];
            _apiSecret = configuration["OKX:ApiSecret"];
            _passphrase = configuration["OKX:Passphrase"];
        }
        public async Task<Okxkey> GetByUserIdAsync(int userId) =>
           await _context.Okxkeys.FirstOrDefaultAsync(k => k.UserId == userId && k.IsActive == true);

      

        public async Task<string> CreateSubAccountAsync(string subAccountName, string type, string label)
        {
            var method = "POST";
            var requestPath = "/api/v5/users/subaccount/create-subaccount";

            var bodyObject = new
            {
                subAcct = subAccountName,
                type = type,
                label = label
            };

            var body = JsonConvert.SerializeObject(bodyObject);

            var timestamp = DateTimeOffset.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var signature = CreateSignature(timestamp, method, requestPath, body, _apiSecret);

            var request = new HttpRequestMessage(HttpMethod.Post, "https://www.okx.com" + requestPath)
            {
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            };

            request.Headers.Add("OK-ACCESS-KEY", _apiKey);
            request.Headers.Add("OK-ACCESS-SIGN", signature);
            request.Headers.Add("OK-ACCESS-TIMESTAMP", timestamp);
            request.Headers.Add("OK-ACCESS-PASSPHRASE", _passphrase);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }


        public string CreateSignature(string timestamp, string method, string requestPath, string body, string secret)
        {
            var prehash = timestamp + method.ToUpper() + requestPath + body;
            var keyBytes = Encoding.UTF8.GetBytes(secret);
            var prehashBytes = Encoding.UTF8.GetBytes(prehash);
            using var hmacsha256 = new HMACSHA256(keyBytes);
            var hash = hmacsha256.ComputeHash(prehashBytes);
            return Convert.ToBase64String(hash);
        }

        public void AddAuthenticationHeaders(HttpRequestMessage request, string method, string path, string body = "")
        {
            var timestamp = DateTimeOffset.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

            var signature = CreateSignature(timestamp, method, path, body, _apiSecret);

            request.Headers.Add("OK-ACCESS-KEY", _apiKey);
            request.Headers.Add("OK-ACCESS-SIGN", signature);
            request.Headers.Add("OK-ACCESS-TIMESTAMP", timestamp);
            request.Headers.Add("OK-ACCESS-PASSPHRASE", _passphrase);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<string> GetSubAccountsAsync(string apiKey, string secretKey, string passphrase)
        {
            string method = "GET";
            string requestPath = "/api/v5/users/subaccount/list";
            string body = "";
            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

            var signature = CreateSignature(timestamp, method, requestPath, body, secretKey);

            var request = new HttpRequestMessage(HttpMethod.Get, "https://www.okx.com" + requestPath);
            request.Headers.Add("OK-ACCESS-KEY", apiKey);
            request.Headers.Add("OK-ACCESS-SIGN", signature);
            request.Headers.Add("OK-ACCESS-TIMESTAMP", timestamp);
            request.Headers.Add("OK-ACCESS-PASSPHRASE", passphrase);
            request.Headers.Add("Accept", "application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }


        public async Task<string> GetTickerAsync(string instId)
        {
            var url = $"https://www.okx.com/api/v5/market/ticker?instId={instId}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
