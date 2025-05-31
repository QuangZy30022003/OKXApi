using Newtonsoft.Json;
using Repositories.Enities.Handlers;
using Repositories.Enities.OKx;
using Repositories.Interfaces;
using Repositories.Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class WebhookService : IWebhookService
    {
        private readonly IOkxApiService _okxService;
        private readonly IWebhookEventRepository _webhookRepo;
        private readonly ITradeLogRepository _tradeRepo;

        public WebhookService(IOkxApiService okxService, IWebhookEventRepository webhookEventRepository, ITradeLogRepository tradeLogRepository)
        {
            _okxService = okxService;
            _webhookRepo = webhookEventRepository;
            _tradeRepo = tradeLogRepository;
        }


        public async Task<bool> HandleCustomOrderAsync(OKXOrderRequest dto)
        {
            try
            {
                var order = new OKXOrderRequest
                {
                    InstId = dto.InstId,
                    TdMode = dto.TdMode,
                    ClOrdId = dto.ClOrdId,
                    Side = dto.Side,
                    OrdType = dto.OrdType,
                    Px = dto.Px,
                    Sz = dto.Sz
                };

                var result = await _okxService.PlaceOrderAsync(order);

                await _webhookRepo.AddAsync(new WebhookEvent
                {
                    Type = $"{dto.Side.ToUpper()}_{dto.OrdType.ToUpper()}",
                    Price = decimal.TryParse(dto.Px, out var price) ? price : 0,
                    ReceivedAt = DateTime.UtcNow,
                    Status = result ? "Success" : "Failed"
                });

                return result;
            }
            catch (Exception ex)
            {
                // Lưu log lỗi chi tiết vào database hoặc file log
                await _webhookRepo.AddAsync(new WebhookEvent
                {
                    Type = "Exception",
                    Price = 0,
                    ReceivedAt = DateTime.UtcNow,
                    Status = "Failed",
                    // Có thể thêm trường để lưu message lỗi nếu có
                    // ErrorMessage = ex.Message
                });

                Console.WriteLine($"[ERROR] Exception in HandleCustomOrderAsync: {ex.Message}");
                throw; // để controller có thể trả lỗi 500 với message chi tiết
            }
        }


    }
}
