using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Store.Core.Entities;
using Store.Core.Interfaces;
using Stripe;

namespace Store.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartService _cartService;
        private readonly IConfiguration _config;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(IUnitOfWork unitOfWork, ICartService cartService, IConfiguration config, ILogger<PaymentService> logger)
        {
            _cartService = cartService;
            _unitOfWork = unitOfWork;
            _config = config;
            _logger = logger;
        }

        public async Task<Cart?> CreateOrUpdateStripePaymentIntent(string buyerId)
        {
            StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];

            var cart = await _cartService.GetCartAsync(buyerId);
            if (cart == null) return null;

            var shippingPrice = 500m; // Example shipping price in cents

            var service = new PaymentIntentService();
            PaymentIntent intent;

            var amount = (long)(cart.Items.Sum(i => i.Quantity * (i.Product.Price * 100)) + shippingPrice);

            if (string.IsNullOrEmpty(cart.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = amount,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                intent = await service.CreateAsync(options);
                cart.PaymentIntentId = intent.Id;
                cart.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = amount
                };
                await service.UpdateAsync(cart.PaymentIntentId, options);
            }

            return cart;
        }

        public async Task<string?> CreatePayPalOrder(string buyerId)
        {
            var cart = await _cartService.GetCartAsync(buyerId);
            if (cart == null) return null;

            // In a real app, you would use PayPal SDK here.
            // For now, we return a simulated PayPal Order ID.
            var orderId = "PAYPAL-" + Guid.NewGuid().ToString().ToUpper().Substring(0, 8);
            
            // Log the "creation"
            _logger.LogInformation("Created simulated PayPal order {OrderId} for user {BuyerId}", orderId, buyerId);

            return orderId;
        }
    }
}
