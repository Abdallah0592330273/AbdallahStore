using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.API.Errors;
using Store.ModelView;
using Store.Core.Entities;
using Store.Core.Interfaces;

namespace Store.API.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _mapper = mapper;
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderCreateDto orderDto)
        {
            var email = GetCurrentUserEmail();
            if (email == null) return Unauthorized(new ApiResponse(401));

            var address = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);

            var order = await _orderService.CreateOrderAsync(email, orderDto.DeliveryMethodId, orderDto.CartId, address);

            if (order == null) return BadRequest(new ApiResponse(400, "Problem creating order"));

            return Ok(_mapper.Map<Order, OrderToReturnDto>(order));
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var email = GetCurrentUserEmail();
            if (email == null) return Unauthorized(new ApiResponse(401));

            var orders = await _orderService.GetOrdersForUserAsync(email);

            return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id)
        {
            var email = GetCurrentUserEmail();
            if (email == null) return Unauthorized(new ApiResponse(401));

            var order = await _orderService.GetOrderByIdAsync(id, email);

            if (order == null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Order, OrderToReturnDto>(order));
        }

        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            return Ok(await _orderService.GetDeliveryMethodsAsync());
        }

        // Admin Endpoints
        [HttpGet("admin")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetAllOrdersForAdmin()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders));
        }

        [HttpPut("admin/{id}/status")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<OrderToReturnDto>> UpdateOrderStatus(int id, [FromBody] string status)
        {
            if (!Enum.TryParse<OrderStatus>(status, true, out var orderStatus))
                return BadRequest(new ApiResponse(400, "Invalid status"));

            var order = await _orderService.UpdateOrderStatusAsync(id, orderStatus);

            if (order == null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Order, OrderToReturnDto>(order));
        }
    }
}
