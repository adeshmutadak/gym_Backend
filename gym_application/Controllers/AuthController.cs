using System.Security.Claims;
using CommonLayer.CommonResponse;
using Dto.Request;
using Dto.Response;
using gym_application.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Services;
namespace gym_application.Controllers
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>API 1 - Admin login. Returns a token with the Admin role.</summary>
        [HttpPost("admin-login")]
        [AllowAnonymous]
        [ApiKey]
        public GeneralResponse<LoginResponse> AdminLogin(AdminLoginRequest request)
            => _authService.AdminLogin(request);

        /// <summary>API 2 - Admin adds a customer or trainer. Requires the Admin token.</summary>
        [HttpPost("add-user")]
        [Authorize(Policy = "AdminOnly")]
        [ApiKey]
        public async Task<GeneralResponse<RegistrationResponse>> AddUser(RegistrationRequestDto request)
            => await _authService.AddUser(request);

        /// <summary>API 3 - Customer or trainer login.</summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<GeneralResponse<LoginResponse>> UserLogin(LogRequest request)
            => await _authService.UserLogin(request);

        /// <summary>API 4 - the signed-in user sets their own password.</summary>
        [HttpPost("reset-password")]
        [Authorize]
        public async Task<BaseResponse> ResetPassword(ResetPasswordRequestt request)
        {
        //    var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            return await _authService.ResetPassword(request.UserId, request);
        }
    }
}
