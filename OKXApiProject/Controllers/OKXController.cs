using Microsoft.AspNetCore.Mvc;
using Repositories.Enities;
using Repositories.Enities.Subaacount;
using Repositories.Models;
using Services.Interfaces;
using Services.Service;
using System.Text.Json;

namespace OKXApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OKXController : ControllerBase
    {
        private readonly IOKXService _okxApiService;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _config;
        private readonly ISubAccountService _subAccountService;
        private readonly IOKXService _okxService;
        public OKXController(IOKXService okxApiService, IJwtService jwtService, IConfiguration config, ISubAccountService subAccountService, IOKXService okxService)
        {
            _okxApiService = okxApiService;
            _jwtService = jwtService;
            _config = config;
            _subAccountService = subAccountService;
            _okxService = okxService;
        }

        [HttpGet("balance")]
        public async Task<IActionResult> GetBalance()
        {
            var result = await _okxApiService.GetBalanceAsync();
            return Ok(JsonDocument.Parse(result));
        }

        [HttpGet("trades/{instType}")]
        public async Task<IActionResult> GetTradeHistory(string instType)
        {
            var result = await _okxApiService.GetTradeHistoryAsync(instType);
            return Ok(JsonDocument.Parse(result));
        }

        [HttpPost("subaccount")]
        public async Task<IActionResult> CreateSubAccount([FromBody] CreateSubAccountRequest request)
        {
            try
            {
                var success = await _okxApiService.CreateSubAccountAsync(request.SubAccountName, request.Type, request.Label, request.ApiKey);
                return Ok(new { success = success, message = "Tạo sub-account thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("trend")]
        public async Task<IActionResult> GetTrend([FromQuery] string instId)
        {
            try
            {
                var trend = await _okxService.GetPriceTrendAsync(instId);
                return Ok(new { instId, trend });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
