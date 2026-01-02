using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.API.Errors;
using Store.ModelView;
using Store.Core.Entities;
using Store.Core.Interfaces;

namespace Store.API.Controllers
{
    public class BrandsController : BaseApiController
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public BrandsController(IProductService productService, IMapper mapper)
        {
            _mapper = mapper;
            _productService = productService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IReadOnlyList<BrandDto>>> GetBrands()
        {
            var brands = await _productService.GetBrandsAsync();
            return Ok(_mapper.Map<IReadOnlyList<ProductBrand>, IReadOnlyList<BrandDto>>(brands));
        }

        // Admin Endpoints
        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<BrandDto>> CreateBrand(BrandDto brandDto)
        {
            var brand = _mapper.Map<BrandDto, ProductBrand>(brandDto);
            var result = await _productService.CreateBrandAsync(brand);
            return Ok(_mapper.Map<ProductBrand, BrandDto>(result));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<BrandDto>> UpdateBrand(int id, BrandDto brandDto)
        {
            var brand = _mapper.Map<BrandDto, ProductBrand>(brandDto);
            var result = await _productService.UpdateBrandAsync(id, brand);
            if (result == null) return NotFound(new ApiResponse(404));
            return Ok(_mapper.Map<ProductBrand, BrandDto>(result));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult> DeleteBrand(int id)
        {
            var result = await _productService.DeleteBrandAsync(id);
            if (!result) return NotFound(new ApiResponse(404));
            return Ok();
        }
    }
}
