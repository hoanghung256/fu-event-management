using FUEM.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using Net.payOS;
using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Google.Apis.Requests.BatchRequest;

namespace FUEM.Infrastructure.Common
{
    public class PayOSService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly PayOS _payOS;
        private readonly IConfiguration _configuration;

        public PayOSService(IConfiguration configuration, HttpClient httpClient, IHttpContextAccessor httpContextAccessor, PayOS payOS)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _payOS = payOS;
            _configuration = configuration;
        }

        public async Task<string> CreatePaymentUrlForEventTicket(Event registeringEvent)
        {
            int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
            ItemData item = new ItemData($"Ticket Pay for Event: {registeringEvent.Fullname}", 1, registeringEvent.TicketPrice ?? 0);
            List<ItemData> items = new List<ItemData> { item };

            // Get the current request's base URL
            var request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";

            PaymentData paymentData = new(
                orderCode,
                registeringEvent.TicketPrice ?? 0,
                $"Ticket Payment",
                items,
                $"{baseUrl}/EventRegistration/TicketCheckoutCallback",
                $"{baseUrl}/EventRegistration/TicketCheckoutCallback",
                null, null, null, null, null,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 5 * 60  // Expired in 5 minutes
            );

            
            CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);

            return createPayment.checkoutUrl;
        }

        public async Task<PayOSJsonResponse?> VerifyPayment(string paymentId)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("x-api-key", _configuration["PayOS:ApiKey"]);
            httpClient.DefaultRequestHeaders.Add("x-client-id", _configuration["PayOS:ClientId"]);

            var response = await httpClient.GetAsync($"https://api-merchant.payos.vn/v2/payment-requests/{paymentId}");

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var result = await response.Content.ReadFromJsonAsync<PayOSJsonResponse>();

                return result;
            }
            return null;
        }

        public class PayOSJsonResponse
        {
            public string Code { get; set; }
            public string Desc { get; set; }
            public ResponseData Data { get; set; }
            public string Signature { get; set; }
        }

        public class ResponseData
        {
            public string Id { get; set; }
            public int OrderCode { get; set; }
            public int Amount { get; set; }
            public int AmountPaid { get; set; }
            public int AmountRemaining { get; set; }
            public string Status { get; set; }
            public DateTime CreatedAt { get; set; }
            public List<Transaction> Transactions { get; set; }
            public DateTime? CanceledAt { get; set; }
            public string CancellationReason { get; set; }
        }

        public class Transaction
        {
        }
    }
}
