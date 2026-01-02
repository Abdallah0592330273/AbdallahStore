using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.API.Errors;
using Store.ModelView;
using Store.Core.Interfaces;

namespace Store.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BaseApiController : ControllerBase
    {
        protected string? GetCurrentUserEmail()
        {
            return User.FindFirstValue(ClaimTypes.Email);
        }

        protected string? GetCurrentUserName()
        {
            return User.FindFirstValue(ClaimTypes.GivenName) ?? User.FindFirstValue(ClaimTypes.Name);
        }

        protected ActionResult<UserDto> HandleAuthResult(AuthResponse result, string? email = null, string? displayName = null)
        {
            if (!result.IsSuccess)
            {
                if (result.Message == "Unauthorized" || result.Message == "Invalid password")
                {
                    return Unauthorized(new ApiResponse(401, result.Message));
                }
                return BadRequest(new ApiResponse(400, result.Message));
            }

            return new UserDto
            {
                Email = email ?? string.Empty,
                DisplayName = displayName ?? "User",
                Token = result.Token!
            };
        }
    }
}
