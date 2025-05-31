using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IOKXKeyRepository
    {
        Task<Okxkey> GetByUserIdAsync(int userId);

        Task<string> CreateSubAccountAsync(string subAccountName, string type, string label);
        void AddAuthenticationHeaders(HttpRequestMessage request, string method, string path, string body = "");

        Task<string> GetSubAccountsAsync(string apiKey, string secretKey, string passphrase);

        Task<string> GetTickerAsync(string instId);
    }
}
