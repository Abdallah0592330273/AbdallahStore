using Microsoft.AspNetCore.Http;
using Store.Core.Entities;

namespace Store.Core.Interfaces
{
    public interface IProductService
    {
        Task<IReadOnlyList<Product>> GetProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<IReadOnlyList<ProductBrand>> GetBrandsAsync();
        Task<IReadOnlyList<ProductType>> GetTypesAsync();
        Task<IReadOnlyList<Category>> GetCategoriesAsync();

        // Admin Methods
        Task<Product> CreateProductAsync(Product product);
        Task<Product?> UpdateProductAsync(int id, Product product);
        Task<bool> DeleteProductAsync(int id);
        Task<Product?> SetProductPhotoAsync(int id, IFormFile file);

        // Admin Methods for Brands/Types
        Task<ProductBrand> CreateBrandAsync(ProductBrand brand);
        Task<ProductBrand?> UpdateBrandAsync(int id, ProductBrand brand);
        Task<bool> DeleteBrandAsync(int id);

        Task<ProductType> CreateTypeAsync(ProductType type);
        Task<ProductType?> UpdateTypeAsync(int id, ProductType type);
        Task<bool> DeleteTypeAsync(int id);
    }
}
