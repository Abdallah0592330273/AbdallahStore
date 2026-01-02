using Store.Core.Entities;

namespace Store.Core.Interfaces
{
    public interface ICartService
    {
        Task<Cart?> GetCartAsync(string buyerId);
        Task<Cart> CreateCartAsync(string buyerId);
        Task<Cart?> AddItemToCartAsync(string buyerId, int productId, int quantity);
        Task<bool> RemoveItemFromCartAsync(string buyerId, int productId);
        Task<bool> DeleteCartAsync(string buyerId);
    }
}
