using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface ISubAccountRepository
    {
        Task AddAsync(SubAccount subAccount);

        Task<Okxkey?> GetOkxKeyByApiKeyAsync(string apiKey);


    }
}
