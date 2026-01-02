using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.Core.Entities;
using Store.Core.Interfaces;

namespace Store.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IPhotoService _photoService;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenService tokenService, IPhotoService photoService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _photoService = photoService;
        }

        public async Task<AuthResponse> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return new AuthResponse(false, null, "User not found");

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (!result.Succeeded) return new AuthResponse(false, null, "Invalid password");

            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.CreateToken(user, roles);

            return new AuthResponse(true, token, "Login successful");
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var userExists = await _userManager.FindByEmailAsync(request.Email);
            if (userExists != null) return new AuthResponse(false, null, "Email already in use");

            var user = new ApplicationUser
            {
                DisplayName = request.DisplayName,
                Email = request.Email,
                UserName = request.UserName
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded) 
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new AuthResponse(false, null, errors);
            }

            // Default role for registration is Customer
            await _userManager.AddToRoleAsync(user, "Customer");

            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.CreateToken(user, roles);

            return new AuthResponse(true, token, "Registration successful");
        }

        public async Task<AuthResponse> PromoteToAdminAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return new AuthResponse(false, null, "User not found");

            var result = await _userManager.AddToRoleAsync(user, "Admin");

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new AuthResponse(false, null, errors);
            }

            return new AuthResponse(true, null, $"User {email} promoted to Admin successfully");
        }

        public async Task<IReadOnlyList<ApplicationUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<bool> UpdateUserRolesAsync(string email, string[] roles)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return false;

            var userRoles = await _userManager.GetRolesAsync(user);
            
            var result = await _userManager.RemoveFromRolesAsync(user, userRoles);
            if (!result.Succeeded) return false;

            result = await _userManager.AddToRolesAsync(user, roles);
            return result.Succeeded;
        }

        public async Task<string?> SetUserPhotoAsync(string email, IFormFile file)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return null;

            if (file != null)
            {
                var photoResult = await _photoService.AddPhotoAsync(file);

                if (photoResult.Error != null) return null;

                if (!string.IsNullOrEmpty(user.PicturePublicId))
                {
                    await _photoService.DeletePhotoAsync(user.PicturePublicId);
                }

                user.PictureUrl = photoResult.Url;
                user.PicturePublicId = photoResult.PublicId;

                await _userManager.UpdateAsync(user);
                return user.PictureUrl;
            }

            return null;
        }
    }
}
