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
    public class WebhookEventRepository : IWebhookEventRepository
    {
        private readonly ApplicationDbContext _context;
        public WebhookEventRepository(ApplicationDbContext context) => _context = context;

        public async Task<WebhookEvent> AddAsync(WebhookEvent webhook)
        {
            _context.WebhookEvents.Add(webhook);
            await _context.SaveChangesAsync();
            return webhook;
        }

        public async Task UpdateAsync(WebhookEvent webhook)
        {
            _context.WebhookEvents.Update(webhook);
            await _context.SaveChangesAsync();
        }
    }
}
