using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class SubAccountRepository : ISubAccountRepository
    {
        private readonly ApplicationDbContext _context;
        public SubAccountRepository(ApplicationDbContext context) => _context = context;

        public async Task AddAsync(SubAccount subAccount)
        {
            _context.SubAccounts.Add(subAccount);
            await _context.SaveChangesAsync();
        }

        public async Task<Okxkey?> GetOkxKeyByApiKeyAsync(string apiKey)
        {
            return await _context.Okxkeys
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.ApiKey == apiKey && x.IsActive == true);
        }
     
    }
}
