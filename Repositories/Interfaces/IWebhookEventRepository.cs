using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IWebhookEventRepository
    {
        Task<WebhookEvent> AddAsync(WebhookEvent webhook);
        Task UpdateAsync(WebhookEvent webhook);
    }
}
