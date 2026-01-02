using Store.Core.Entities;

namespace Store.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<IReadOnlyList<Category>> GetCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<Category> CreateCategoryAsync(Category category);
        Task<Category?> UpdateCategoryAsync(int id, Category category);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
