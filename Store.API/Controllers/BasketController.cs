using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Store.ModelView;
using Store.Core.Entities;
using Store.Core.Interfaces;

namespace Store.API.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;

        public BasketController(ICartService cartService, IMapper mapper)
        {
            _mapper = mapper;
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<ActionResult<CartDto>> GetBasketById()
        {
            var buyerId = GetCurrentUserEmail() ?? "anonymous"; // Simplification
            var basket = await _cartService.GetCartAsync(buyerId);
            return Ok(_mapper.Map<Cart, CartDto>(basket ?? new Cart { BuyerId = buyerId }));
        }

        [HttpPost]
        public async Task<ActionResult<CartDto>> AddToBasket(int productId, int quantity = 1)
        {
            var buyerId = GetCurrentUserEmail() ?? "anonymous";
            var basket = await _cartService.AddItemToCartAsync(buyerId, productId, quantity);
            if (basket == null) return BadRequest("Could not add item to basket");
            return Ok(_mapper.Map<Cart, CartDto>(basket));
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteBasket()
        {
            var buyerId = GetCurrentUserEmail() ?? "anonymous";
            await _cartService.DeleteCartAsync(buyerId);
            return Ok();
        }
    }
}
