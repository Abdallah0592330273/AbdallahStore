using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Store.API.Errors;
using Store.ModelView;
using Store.Core.Entities;
using Store.Core.Interfaces;

namespace Store.API.Controllers
{
    public class AddressController : BaseApiController
    {
        private readonly IAddressService _addressService;
        private readonly IMapper _mapper;

        public AddressController(IAddressService addressService, IMapper mapper)
        {
            _mapper = mapper;
            _addressService = addressService;
        }

        [HttpGet]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var email = GetCurrentUserEmail();
            if (email == null) return Unauthorized(new ApiResponse(401));

            var address = await _addressService.GetUserAddressAsync(email);
            if (address == null) return NotFound(new ApiResponse(404, "Address not found"));

            return Ok(_mapper.Map<Address, AddressDto>(address));
        }

        [HttpPut]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto addressDto)
        {
            var email = GetCurrentUserEmail();
            if (email == null) return Unauthorized(new ApiResponse(401));

            var address = _mapper.Map<AddressDto, Address>(addressDto);
            var updatedAddress = await _addressService.UpdateUserAddressAsync(email, address);

            if (updatedAddress == null) return BadRequest(new ApiResponse(400, "Problem updating the user address"));

            return Ok(_mapper.Map<Address, AddressDto>(updatedAddress));
        }
    }
}
