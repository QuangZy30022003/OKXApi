using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Services.Interfaces;
using Services.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection service)
        {
            service.AddScoped<IOKXService, OKXService>();
            service.AddScoped<IJwtService, JwtService>();
            service.AddScoped<ISubAccountService, SubAccountService>();
            service.AddScoped<IWebhookService, WebhookService>();
            service.AddScoped<IOkxApiService, OkxApiService>();
            return service;
        }
    }
}
