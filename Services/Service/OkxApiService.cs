using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Repositories.Enities.OKx;
using Services.Interfaces;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Services.Service
{
    public class OkxApiService : IOkxApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;
        private readonly string _apiKey;
        private readonly string _secretKey;
        private readonly string _passPhrase;

        public OkxApiService(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
            _apiKey = _config["Okx:ApiKey"];
            _secretKey = _config["Okx:ApiSecret"];
            _passPhrase = _config["Okx:Passphrase"];
        }

        public async Task<bool> PlaceOrderAsync(OKXOrderRequest requestData)
        {
            if (string.IsNullOrWhiteSpace(_apiKey) || string.IsNullOrWhiteSpace(_secretKey) || string.IsNullOrWhiteSpace(_passPhrase))
                throw new InvalidOperationException("Missing API key, secret, or passphrase in configuration.");

            var client = _httpClientFactory.CreateClient();
            var url = "https://www.okx.com/api/v5/trade/order";
            var requestPath = "/api/v5/trade/order";

            var filteredRequest = new
            {
                instId = requestData.InstId,
                tdMode = requestData.TdMode,
                clOrdId = requestData.ClOrdId,
                side = requestData.Side,
                ordType = requestData.OrdType,
                px = requestData.Px,
                sz = requestData.Sz
            };

            var json = JsonConvert.SerializeObject(filteredRequest);

            if (string.IsNullOrEmpty(json))
                throw new ArgumentNullException(nameof(json), "Request body is null or empty");

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

            string sign;
            try
            {
                sign = ComputeSignature(timestamp, "POST", requestPath, json, _secretKey);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error computing signature: " + ex.Message, ex);
            }

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };

            request.Headers.Add("OK-ACCESS-KEY", _apiKey);
            request.Headers.Add("OK-ACCESS-SIGN", sign);
            request.Headers.Add("OK-ACCESS-TIMESTAMP", timestamp);
            request.Headers.Add("OK-ACCESS-PASSPHRASE", _passPhrase);
            // request.Headers.Add("x-simulated-trading", "1"); // Uncomment only if using simulated mode

            // Debug logs
            Console.WriteLine("Sending request to OKX...");
            Console.WriteLine($"Timestamp: {timestamp}");
            Console.WriteLine($"Sign: {sign}");
            Console.WriteLine($"API Key: {_apiKey}");
            Console.WriteLine($"Passphrase: {_passPhrase}");
            Console.WriteLine($"Request Body: {json}");

            var response = await client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Response from OKX:");
            Console.WriteLine(responseString);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[ERROR] HTTP {(int)response.StatusCode}: {response.ReasonPhrase}");
                return false;
            }

            try
            {
                var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseString);
                if (jsonResponse.TryGetProperty("code", out var codeElement))
                {
                    var code = codeElement.GetString();
                    return code == "0";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR] Failed to parse JSON response: " + ex.Message);
            }

            return false;
        }

        public static string ComputeSignature(string timestamp, string method, string requestPath, string body, string secretKey)
        {
            if (timestamp == null) throw new ArgumentNullException(nameof(timestamp));
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (requestPath == null) throw new ArgumentNullException(nameof(requestPath));
            if (body == null) throw new ArgumentNullException(nameof(body));
            if (secretKey == null) throw new ArgumentNullException(nameof(secretKey));

            string preHash = timestamp + method.ToUpper() + requestPath + body;
            byte[] keyBytes = Encoding.UTF8.GetBytes(secretKey);
            byte[] messageBytes = Encoding.UTF8.GetBytes(preHash);

            using var hmac = new HMACSHA256(keyBytes);
            return Convert.ToBase64String(hmac.ComputeHash(messageBytes));
        }
    }
}
