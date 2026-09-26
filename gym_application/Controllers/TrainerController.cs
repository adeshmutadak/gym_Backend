using System.Security.Claims;
using CommonLayer.CommonResponse;
using Dto.Request;
using Dto.Response;
using gym_application.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace gym_application.Controllers
{

    [Route("v1/api/[controller]")]
    [ApiController]
    public class TrainerController : ControllerBase
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        private int CallerUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        private bool CallerIsAdmin => User.IsInRole("Admin");

        /// <summary>Trainer adds their own details, or the Admin adds them by passing userId.</summary>
        [HttpPost("profile")]
        [Authorize(Roles = "Admin,Trainer")]
        public async Task<GeneralResponse<TrainerProfileResponse>> CreateProfile(
            CreateTrainerProfileRequest request)
            => await _trainerService.CreateProfile(request, CallerUserId, CallerIsAdmin);

        /// <summary>The signed-in trainer's own profile.</summary>
        [HttpGet("profile/me")]
        [Authorize(Roles = "Trainer")]
        public async Task<GeneralResponse<TrainerProfileResponse>> GetMyProfile()
            => await _trainerService.GetProfile(CallerUserId);

        /// <summary>Any trainer's profile, by user id. Admin only.</summary>
        [HttpGet("profile/{userId:int}")]
        [Authorize(Policy = "AdminOnly")]
        [ApiKey]
        public async Task<GeneralResponse<TrainerProfileResponse>> GetProfile(int userId)
            => await _trainerService.GetProfile(userId);

        /// <summary>All trainers. Admin only.</summary>
        [HttpGet("all")]
        [Authorize(Policy = "AdminOnly")]
        [ApiKey]
        public async Task<GeneralResponse<List<TrainerProfileResponse>>> GetAll()
            => await _trainerService.GetAll();

        /// <summary>Trainer updates their own profile.</summary>
        [HttpPut("profile/me")]
        [Authorize(Roles = "Trainer")]
        public async Task<GeneralResponse<TrainerProfileResponse>> UpdateMyProfile(
            UpdateTrainerProfileRequest request)
            => await _trainerService.UpdateProfile(request, CallerUserId, callerIsAdmin: false);

        /// <summary>Admin updates any trainer's profile.</summary>
        [HttpPut("profile/{userId:int}")]
        [Authorize(Policy = "AdminOnly")]
        [ApiKey]
        public async Task<GeneralResponse<TrainerProfileResponse>> UpdateProfile(
            int userId, UpdateTrainerProfileRequest request)
            => await _trainerService.UpdateProfile(request, userId, callerIsAdmin: true);
    }
}
