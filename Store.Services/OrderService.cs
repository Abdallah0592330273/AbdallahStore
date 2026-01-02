using Store.Core.Entities;
using Store.Core.Interfaces;
using Store.Core.Specifications;

namespace Store.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartService _cartService;

        public OrderService(IUnitOfWork unitOfWork, ICartService cartService)
        {
            _cartService = cartService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Order?> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string cartId, Address shippingAddress)
        {
            // 1. Get cart
            var cart = await _cartService.GetCartAsync(buyerEmail); // Logic assumes cart buyerId is user email or similar
            if (cart == null) return null;

            // 2. Get items from product repo
            var items = new List<OrderItem>();
            foreach (var item in cart.Items)
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.ProductId);
                if (productItem == null) continue;

                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
                var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
                items.Add(orderItem);
            }

            // 3. Get delivery method
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            if (deliveryMethod == null) return null;

            // 4. Calculate subtotal
            var subtotal = items.Sum(item => item.Price * item.Quantity);

            // 5. Create order
            var orderAddress = new OrderAddress(shippingAddress.FirstName, shippingAddress.LastName, 
                shippingAddress.Street, shippingAddress.City, shippingAddress.State, shippingAddress.ZipCode);
            
            var order = new Order(items, buyerEmail, orderAddress, deliveryMethod, subtotal, cart.PaymentIntentId ?? "");
            _unitOfWork.Repository<Order>().Add(order);

            // 6. Save to db
            var result = await _unitOfWork.Complete();

            if (result <= 0) return null;

            // 7. Delete cart
            await _cartService.DeleteCartAsync(buyerEmail);

            // 8. Return order
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrdersWithItemsAndDeliveryMethodSpecification(id, buyerEmail);
            return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrdersWithItemsAndDeliveryMethodSpecification(buyerEmail);
            return await _unitOfWork.Repository<Order>().ListAsync(spec);
        }

        public async Task<IReadOnlyList<Order>> GetAllOrdersAsync()
        {
            var spec = new OrdersWithItemsAndDeliveryMethodSpecification();
            return await _unitOfWork.Repository<Order>().ListAsync(spec);
        }

        public async Task<Order?> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(orderId);

            if (order == null) return null;

            order.Status = status;
            _unitOfWork.Repository<Order>().Update(order);

            var result = await _unitOfWork.Complete();

            if (result <= 0) return null;

            return order;
        }
    }
}
