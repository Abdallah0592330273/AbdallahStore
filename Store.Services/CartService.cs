using Microsoft.EntityFrameworkCore;
using Store.Core.Entities;
using Store.Core.Interfaces;
using Store.Core.Specifications;

namespace Store.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;


        }

        public async Task<Cart?> GetCartAsync(string buyerId)
        {
            return await _unitOfWork.Repository<Cart>()
                .GetEntityWithSpec(new CartWithItemsSpecification(buyerId));
        }

        public async Task<Cart> CreateCartAsync(string buyerId)
        {
            var cart = new Cart { BuyerId = buyerId };
            _unitOfWork.Repository<Cart>().Add(cart);
            await _unitOfWork.Complete();
            return cart;
        }

        public async Task<Cart?> AddItemToCartAsync(string buyerId, int productId, int quantity)
        {
            var cart = await GetCartAsync(buyerId) ?? await CreateCartAsync(buyerId);

            var item = cart.Items.FirstOrDefault(x => x.ProductId == productId);
            if (item == null)
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(productId);
                if (product == null) return null;

                item = new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    CartId = cart.Id
                };
                cart.Items.Add(item);
            }
            else
            {
                item.Quantity += quantity;
            }

            _unitOfWork.Repository<Cart>().Update(cart);
            await _unitOfWork.Complete();
            return cart;
        }

        public async Task<bool> RemoveItemFromCartAsync(string buyerId, int productId)
        {
            var cart = await GetCartAsync(buyerId);
            if (cart == null) return false;

            var item = cart.Items.FirstOrDefault(x => x.ProductId == productId);
            if (item == null) return false;

            cart.Items.Remove(item);
            _unitOfWork.Repository<Cart>().Update(cart);
            return await _unitOfWork.Complete() > 0;
        }

        public async Task<bool> DeleteCartAsync(string buyerId)
        {
            var cart = await GetCartAsync(buyerId);
            if (cart == null) return false;

            _unitOfWork.Repository<Cart>().Delete(cart);
            return await _unitOfWork.Complete() > 0;
        }
    }

    // Temporary specification for demo purposes
    public class CartWithItemsSpecification : BaseSpecification<Cart>
    {
        public CartWithItemsSpecification(string buyerId) : base(x => x.BuyerId == buyerId)
        {
            AddInclude(x => x.Items);
            // AddInclude("Items.Product"); // EF Core style
        }
    }
}
