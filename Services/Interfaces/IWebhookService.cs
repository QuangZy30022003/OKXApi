using Repositories.Enities.Handlers;
using Repositories.Enities.OKx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IWebhookService
    {
        //Task ProcessWebhookAsync(OKXOrderRequest dto);

        Task<bool> HandleCustomOrderAsync(OKXOrderRequest dto);
    }
}
