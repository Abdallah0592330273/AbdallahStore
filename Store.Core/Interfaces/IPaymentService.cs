using Store.Core.Entities;

namespace Store.Core.Interfaces
{
    public interface IPaymentService
    {
        Task<Cart?> CreateOrUpdateStripePaymentIntent(string cartId);
        Task<string?> CreatePayPalOrder(string cartId);
    }
}
