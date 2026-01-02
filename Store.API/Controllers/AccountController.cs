using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.API.Errors;
using Store.ModelView;
using Store.Core.Interfaces;

namespace Store.API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto.Email, loginDto.Password);
            return HandleAuthResult(result, loginDto.Email);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> Register(RegisterRequest registerRequest)
        {
            var result = await _authService.RegisterAsync(registerRequest);
            return HandleAuthResult(result, registerRequest.Email, registerRequest.DisplayName);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("promote/{email}")]
        public async Task<ActionResult<AuthResponse>> PromoteToAdmin(string email)
        {
            var result = await _authService.PromoteToAdminAsync(email);
            if (!result.IsSuccess) 
            return BadRequest(new ApiResponse(400, result.Message));
            return Ok(result);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("users")]
        public async Task<ActionResult<IReadOnlyList<UserDto>>> GetAllUsers()
        {
            var users = await _authService.GetAllUsersAsync();
            // In a real app, Map ApplicationUser to UserDto properly (excluding sensitive info)
            return Ok(users.Select(u => new UserDto { Email = u.Email!, DisplayName = u.DisplayName }));
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPut("users/{email}/roles")]
        public async Task<ActionResult> UpdateUserRoles(string email, UserUpdateRolesDto rolesDto)
        {
            var result = await _authService.UpdateUserRolesAsync(email, rolesDto.Roles);
            if (!result) return BadRequest(new ApiResponse(400, "Problem updating user roles"));
            return Ok();
        }

        [Authorize]
        [HttpPut("photo")]
        public async Task<ActionResult<string>> AddUserPhoto(IFormFile file)
        {
            var email = GetCurrentUserEmail();
            if (email == null) return Unauthorized(new ApiResponse(401));

            var result = await _authService.SetUserPhotoAsync(email, file);
            if (result == null) return BadRequest(new ApiResponse(400, "Problem adding photo"));

            return Ok(new { url = result });
        }
    }

    public class UserUpdateRolesDto
    {
        public string[] Roles { get; set; } = Array.Empty<string>();
    }

    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
