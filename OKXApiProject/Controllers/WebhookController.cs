using Microsoft.AspNetCore.Mvc;
using Repositories.Enities.Handlers;
using Repositories.Enities.OKx;
using Services.Interfaces;
using Services.Service;

namespace OKXApiProject.Controllers
{
    [ApiController]
    [Route("api/webhook")]
    public class WebhookController : Controller
    {
        private readonly IWebhookService _service;
        private readonly ISubAccountService _subAccountService;
        public WebhookController(IWebhookService service, ISubAccountService subAccountService)
        {
            _service = service;
            _subAccountService = subAccountService;
        }

        [HttpPost("place")]
        public async Task<IActionResult> PlaceOrder([FromBody] OKXOrderRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var success = await _service.HandleCustomOrderAsync(request);

                if (success)
                    return Ok(new { message = "Order placed successfully" });
                else
                    return StatusCode(500, new { message = "Failed to place order" });
            }
            catch (Exception ex)
            {
                // Trả lỗi chi tiết cho client (hoặc chỉ message an toàn nếu cần)
                return StatusCode(500, new { message = "Exception: " + ex.Message });
            }
        }

        [HttpGet("subaccounts")]
        public async Task<IActionResult> GetSubAccounts([FromQuery] string apiKey, [FromQuery] string secretKey, [FromQuery] string passphrase)
        {
            try
            {
                var result = await _subAccountService.GetSubAccountsAsync(apiKey, secretKey, passphrase);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
