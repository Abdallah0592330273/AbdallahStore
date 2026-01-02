using Store.Core.Entities;

namespace Store.Core.Interfaces
{
    public interface IUserRepository : IGenericRepository<ApplicationUser>
    {
        Task<ApplicationUser?> GetByEmailAsync(string email);
        // Add other user-specific methods here
    }
}
