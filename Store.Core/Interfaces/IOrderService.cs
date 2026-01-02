using Store.Core.Entities;

namespace Store.Core.Interfaces
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string cartId, Address shippingAddress);
        Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);
        Task<Order?> GetOrderByIdAsync(int id, string buyerEmail);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
        
        // Admin Methods
        Task<IReadOnlyList<Order>> GetAllOrdersAsync();
        Task<Order?> UpdateOrderStatusAsync(int orderId, OrderStatus status);
    }
}
