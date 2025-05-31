using Microsoft.Extensions.Configuration;
using Repositories.Enities;
using Repositories.Enities.Subaacount;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ISubAccountService
    {

        Task<List<OkxSubAccountDto>> GetSubAccountsAsync(string apiKey, string secretKey, string passphrase);
    }

}
