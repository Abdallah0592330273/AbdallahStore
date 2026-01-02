using Microsoft.AspNetCore.Http;
using Store.Core.Entities;
using Store.Core.Interfaces;

namespace Store.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoService _photoService;
        public ProductService(IUnitOfWork unitOfWork, IPhotoService photoService)
        {
            _photoService = photoService;
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            var spec = new Store.Core.Specifications.ProductsWithTypesAndBrandsSpecification();
            return await _unitOfWork.Repository<Product>().ListAsync(spec);
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            var spec = new Store.Core.Specifications.ProductsWithTypesAndBrandsSpecification(id);
            return await _unitOfWork.Repository<Product>().GetEntityWithSpec(spec);
        }

        public async Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()
        {
            return await _unitOfWork.Repository<ProductBrand>().ListAllAsync();
        }

        public async Task<IReadOnlyList<ProductType>> GetTypesAsync()
        {
            return await _unitOfWork.Repository<ProductType>().ListAllAsync();
        }

        public async Task<IReadOnlyList<Category>> GetCategoriesAsync()
        {
            return await _unitOfWork.Repository<Category>().ListAllAsync();
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            _unitOfWork.Repository<Product>().Add(product);
            await _unitOfWork.Complete();
            return product;
        }

        public async Task<Product?> UpdateProductAsync(int id, Product product)
        {
            var productToUpdate = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            if (productToUpdate == null) return null;

            // Update properties
            productToUpdate.Name = product.Name;
            productToUpdate.Description = product.Description;
            productToUpdate.Price = product.Price;
            productToUpdate.PictureUrl = product.PictureUrl;
            productToUpdate.ProductTypeId = product.ProductTypeId;
            productToUpdate.ProductBrandId = product.ProductBrandId;
            productToUpdate.CategoryId = product.CategoryId;

            _unitOfWork.Repository<Product>().Update(productToUpdate);
            await _unitOfWork.Complete();
            return productToUpdate;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            if (product == null) return false;

            _unitOfWork.Repository<Product>().Delete(product);
            var result = await _unitOfWork.Complete();
            return result > 0;
        }

        public async Task<Product?> SetProductPhotoAsync(int id, IFormFile file)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            if (product == null) return null;

            if (file != null)
            {
                var photoResult = await _photoService.AddPhotoAsync(file);

                if (photoResult.Error != null) return null;

                if (!string.IsNullOrEmpty(product.PicturePublicId))
                {
                    await _photoService.DeletePhotoAsync(product.PicturePublicId);
                }

                product.PictureUrl = photoResult.Url;
                product.PicturePublicId = photoResult.PublicId;

                _unitOfWork.Repository<Product>().Update(product);
                await _unitOfWork.Complete();
            }

            return product;
        }

        // Admin Methods for Brands
        public async Task<ProductBrand> CreateBrandAsync(ProductBrand brand)
        {
            _unitOfWork.Repository<ProductBrand>().Add(brand);
            await _unitOfWork.Complete();
            return brand;
        }

        public async Task<ProductBrand?> UpdateBrandAsync(int id, ProductBrand brand)
        {
            var brandToUpdate = await _unitOfWork.Repository<ProductBrand>().GetByIdAsync(id);
            if (brandToUpdate == null) return null;

            brandToUpdate.Name = brand.Name;
            _unitOfWork.Repository<ProductBrand>().Update(brandToUpdate);
            await _unitOfWork.Complete();
            return brandToUpdate;
        }

        public async Task<bool> DeleteBrandAsync(int id)
        {
            var brand = await _unitOfWork.Repository<ProductBrand>().GetByIdAsync(id);
            if (brand == null) return false;

            _unitOfWork.Repository<ProductBrand>().Delete(brand);
            var result = await _unitOfWork.Complete();
            return result > 0;
        }

        // Admin Methods for Types
        public async Task<ProductType> CreateTypeAsync(ProductType type)
        {
            _unitOfWork.Repository<ProductType>().Add(type);
            await _unitOfWork.Complete();
            return type;
        }

        public async Task<ProductType?> UpdateTypeAsync(int id, ProductType type)
        {
            var typeToUpdate = await _unitOfWork.Repository<ProductType>().GetByIdAsync(id);
            if (typeToUpdate == null) return null;

            typeToUpdate.Name = type.Name;
            _unitOfWork.Repository<ProductType>().Update(typeToUpdate);
            await _unitOfWork.Complete();
            return typeToUpdate;
        }

        public async Task<bool> DeleteTypeAsync(int id)
        {
            var type = await _unitOfWork.Repository<ProductType>().GetByIdAsync(id);
            if (type == null) return false;

            _unitOfWork.Repository<ProductType>().Delete(type);
            var result = await _unitOfWork.Complete();
            return result > 0;
        }
    }
}
