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
    public class TradeLogRepository : ITradeLogRepository
    {
        private readonly ApplicationDbContext _context;
        public TradeLogRepository(ApplicationDbContext context) => _context = context;

        public async Task<TradeLog?> GetCurrentOrderAsync()
        {
            return await _context.TradeLogs.FirstOrDefaultAsync(x => x.IsCurrentOrder);
        }

        public async Task AddAsync(TradeLog trade)
        {
            _context.TradeLogs.Add(trade);
            await _context.SaveChangesAsync();
        }
    }

}
