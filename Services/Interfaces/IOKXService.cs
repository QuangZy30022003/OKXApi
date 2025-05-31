using Repositories.Enities;
using Repositories.Enities.OKx;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IOKXService
    {
        Task<string> GetBalanceAsync();
        Task<string> GetTradeHistoryAsync(string instType );
       // Task<bool> CreateSubAccountAsync(string subAccountName, string type, string label);

        Task<bool> CreateSubAccountAsync(string subAccountName, string type, string label, string apiKey);

        Task<string> GetPriceTrendAsync(string instId);
    }
}

