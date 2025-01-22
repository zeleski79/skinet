using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Infrastructure.Services;

public class PaymentService(IConfiguration config, ICartService cartService,
        IGenericRepository<Core.Entities.Product> productRepo, 
        IGenericRepository<DeliveryMethod> dmRepo) : IPaymentService
{
    public async Task<ShoppingCart?> CreateOrUpdatePaymentIntent(string cartId) {
        StripeConfiguration.ApiKey = config["StripeSettings:SecretKey"]; // Get the key from the confg file
        var cart = await cartService.GetCartAsync(cartId); // Refresh the cart from backend
        if (cart == null) return null; // We don't find the cart
        var shippingPrice = 0m; // Decimal
        if (cart.DeliveryMethodId.HasValue) {
            var deliveryMethod = await dmRepo.GetByIdAsync((int)cart.DeliveryMethodId);
            if (deliveryMethod == null) return null; // We don't find the dm in DB
            shippingPrice = deliveryMethod.Price;
        }
        // Refresh the prices (replace cart prices with prices coming from DB for security reason)
        foreach (var item in cart.Items) {
            var productItem = await productRepo.GetByIdAsync(item.ProductId);
            if (productItem == null) return null; // We don't find the product in DB
            if (item.Price != productItem.Price) {
                item.Price = productItem.Price;
            }
        }
        var service = new PaymentIntentService(); // STRIPE service !!
        PaymentIntent? intent = null;
        // Do we already have a payment intent in the cart ?
        if (string.IsNullOrEmpty(cart.PaymentIntentId)) {
            // Create new intent
            var options = new PaymentIntentCreateOptions {
                Amount = (long)cart.Items.Sum(x => x.Quantity * (x.Price * 100)) + (long)shippingPrice * 100,
                Currency = "usd",
                PaymentMethodTypes = ["card"]
            };
            intent = await service.CreateAsync(options);
            cart.PaymentIntentId = intent.Id;
            cart.ClientSecret = intent.ClientSecret;
        } else {
            // Update intent
            var options = new PaymentIntentUpdateOptions {
                Amount = (long)cart.Items.Sum(x => x.Quantity * (x.Price * 100)) + (long)shippingPrice * 100,
            };
            await service.UpdateAsync(cart.PaymentIntentId, options);
        } 

        await cartService.SetCartAsync(cart);
        return cart;   
    }
}