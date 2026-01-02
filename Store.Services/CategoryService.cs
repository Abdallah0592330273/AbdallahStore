using Store.Core.Entities;
using Store.Core.Interfaces;

namespace Store.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<Category>> GetCategoriesAsync()
        {
            return await _unitOfWork.Repository<Category>().ListAllAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _unitOfWork.Repository<Category>().GetByIdAsync(id);
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            _unitOfWork.Repository<Category>().Add(category);
            await _unitOfWork.Complete();
            return category;
        }

        public async Task<Category?> UpdateCategoryAsync(int id, Category category)
        {
            var existing = await _unitOfWork.Repository<Category>().GetByIdAsync(id);
            if (existing == null) return null;

            existing.Name = category.Name;
            existing.Description = category.Description;

            _unitOfWork.Repository<Category>().Update(existing);
            await _unitOfWork.Complete();
            return existing;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var existing = await _unitOfWork.Repository<Category>().GetByIdAsync(id);
            if (existing == null) return false;

            _unitOfWork.Repository<Category>().Delete(existing);
            return await _unitOfWork.Complete() > 0;
        }
    }
}
