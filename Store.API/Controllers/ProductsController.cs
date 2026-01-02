using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.API.Errors;
using Store.ModelView;
using Store.Core.Entities;
using Store.Core.Interfaces;

namespace Store.API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            _mapper = mapper;
            _productService = productService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            var products = await _productService.GetProductsAsync();
            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(_mapper.Map<Product, ProductToReturnDto>(product));
        }

        [HttpGet("brands")]
        [AllowAnonymous]
        public async Task<ActionResult<IReadOnlyList<BrandDto>>> GetBrands()
        {
            var brands = await _productService.GetBrandsAsync();
            return Ok(_mapper.Map<IReadOnlyList<ProductBrand>, IReadOnlyList<BrandDto>>(brands));
        }

        [HttpGet("types")]
        [AllowAnonymous]
        public async Task<ActionResult<IReadOnlyList<TypeDto>>> GetTypes()
        {
            var types = await _productService.GetTypesAsync();
            return Ok(_mapper.Map<IReadOnlyList<ProductType>, IReadOnlyList<TypeDto>>(types));
        }

        [HttpGet("categories")]
        [AllowAnonymous]
        public async Task<ActionResult<IReadOnlyList<CategoryDto>>> GetCategories()
        {
            var categories = await _productService.GetCategoriesAsync();
            return Ok(_mapper.Map<IReadOnlyList<Category>, IReadOnlyList<CategoryDto>>(categories));
        }

        // Admin Endpoints
        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<ProductToReturnDto>> CreateProduct(ProductCreateDto productDto)
        {
            var product = _mapper.Map<ProductCreateDto, Product>(productDto);
            var result = await _productService.CreateProductAsync(product);
            return Ok(_mapper.Map<Product, ProductToReturnDto>(result));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<ProductToReturnDto>> UpdateProduct(int id, ProductCreateDto productDto)
        {
            var product = _mapper.Map<ProductCreateDto, Product>(productDto);
            var result = await _productService.UpdateProductAsync(id, product);
            if (result == null) return NotFound(new ApiResponse(404));
            return Ok(_mapper.Map<Product, ProductToReturnDto>(result));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result) return NotFound(new ApiResponse(404));
            return Ok();
        }

        [HttpPut("{id}/photo")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<ProductToReturnDto>> AddProductPhoto(int id, IFormFile file)
        {
            var product = await _productService.SetProductPhotoAsync(id, file);
            if (product == null) return BadRequest(new ApiResponse(400, "Problem adding photo"));
            return Ok(_mapper.Map<Product, ProductToReturnDto>(product));
        }
    }
}
