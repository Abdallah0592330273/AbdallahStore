using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.API.Errors;
using Store.ModelView;
using Store.Core.Entities;
using Store.Core.Interfaces;

namespace Store.API.Controllers
{
    public class TypesController : BaseApiController
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public TypesController(IProductService productService, IMapper mapper)
        {
            _mapper = mapper;
            _productService = productService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IReadOnlyList<TypeDto>>> GetTypes()
        {
            var types = await _productService.GetTypesAsync();
            return Ok(_mapper.Map<IReadOnlyList<ProductType>, IReadOnlyList<TypeDto>>(types));
        }

        // Admin Endpoints
        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<TypeDto>> CreateType(TypeDto typeDto)
        {
            var type = _mapper.Map<TypeDto, ProductType>(typeDto);
            var result = await _productService.CreateTypeAsync(type);
            return Ok(_mapper.Map<ProductType, TypeDto>(result));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<TypeDto>> UpdateType(int id, TypeDto typeDto)
        {
            var type = _mapper.Map<TypeDto, ProductType>(typeDto);
            var result = await _productService.UpdateTypeAsync(id, type);
            if (result == null) return NotFound(new ApiResponse(404));
            return Ok(_mapper.Map<ProductType, TypeDto>(result));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult> DeleteType(int id)
        {
            var result = await _productService.DeleteTypeAsync(id);
            if (!result) return NotFound(new ApiResponse(404));
            return Ok();
        }
    }
}
