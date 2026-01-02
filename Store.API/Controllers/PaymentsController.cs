using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.API.Errors;
using Store.ModelView;
using Store.Core.Entities;
using Store.Core.Interfaces;

namespace Store.API.Controllers
{
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;

        public PaymentsController(IPaymentService paymentService, IMapper mapper)
        {
            _mapper = mapper;
            _paymentService = paymentService;
        }

        [HttpPost("stripe/{cartId}")]
        public async Task<ActionResult<CartDto>> CreateOrUpdateStripePaymentIntent(string cartId)
        {
            var cart = await _paymentService.CreateOrUpdateStripePaymentIntent(cartId);

            if (cart == null) return BadRequest(new ApiResponse(400, "Problem with your cart"));

            return Ok(_mapper.Map<Cart, CartDto>(cart));
        }

        [HttpPost("paypal/{cartId}")]
        public async Task<ActionResult<object>> CreatePayPalOrder(string cartId)
        {
            var orderId = await _paymentService.CreatePayPalOrder(cartId);

            if (orderId == null) return BadRequest(new ApiResponse(400, "Problem creating PayPal order"));

            return Ok(new { id = orderId });
        }
    }
}
// Note: webhook handler usually goes here too
