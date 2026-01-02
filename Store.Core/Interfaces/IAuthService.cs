using Microsoft.AspNetCore.Http;
using Store.Core.Entities;

namespace Store.Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(string email, string password);
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> PromoteToAdminAsync(string email);
        Task<IReadOnlyList<ApplicationUser>> GetAllUsersAsync();
        Task<bool> UpdateUserRolesAsync(string email, string[] roles);
        Task<string?> SetUserPhotoAsync(string email, IFormFile file);
    }

    public record AuthResponse(bool IsSuccess, string? Token, string? Message);
    public record RegisterRequest(string DisplayName, string Email, string Password, string UserName);
}
