using Microsoft.Extensions.DependencyInjection;
using Repositories.Interfaces;
using Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepository(this IServiceCollection service)
        {
            service.AddScoped<IOKXKeyRepository, OKXKeyRepository>();
            service.AddScoped<ISubAccountRepository, SubAccountRepository>();
            service.AddScoped<ITradeLogRepository, TradeLogRepository>();
            service.AddScoped<IUserRepository, UserRepository>();
            service.AddScoped<IWebhookEventRepository, WebhookEventRepository>();
            return service;
        }
    }
}
