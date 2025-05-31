using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Repositories.Enities;
using Repositories.Enities.OKx;
using Repositories.Enities.Subaacount;
using Repositories.Interfaces;
using Repositories.Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class SubAccountService : ISubAccountService
    {
        private readonly ISubAccountRepository _subAccountRepository;
        private readonly IOKXKeyRepository _okxRepository;
        public SubAccountService(ISubAccountRepository subAccountRepository, IOKXKeyRepository okxRepository)
        {
            _subAccountRepository = subAccountRepository;
            _okxRepository = okxRepository;
        }

        public async Task<List<OkxSubAccountDto>> GetSubAccountsAsync(string apiKey, string secretKey, string passphrase)
        {
            var rawJson = await _okxRepository.GetSubAccountsAsync(apiKey, secretKey, passphrase);

            var parsed = JsonConvert.DeserializeObject<OkxApiResponse<List<OkxSubAccountDto >>>(rawJson);

            if (parsed == null || parsed.Code != "0")
            {
                var errorMsg = parsed?.Msg ?? "Không phân tích được phản hồi từ OKX";
                throw new Exception($"Lỗi lấy danh sách SubAccount từ OKX: {errorMsg}");
            }

            return parsed.Data;
        }
    }

}
