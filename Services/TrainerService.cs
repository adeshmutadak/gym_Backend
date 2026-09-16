using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CommonLayer.CommonResponse;
using Dto.Request;
using Dto.Response;
using gym_application.Models;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace Services
{
    public class TrainerService : ITrainerService
    {
        private const int CodeRetryAttempts = 3;

        private readonly ITrainerRepo _trainerRepo;

        public TrainerService(ITrainerRepo trainerRepo)
        {
            _trainerRepo = trainerRepo;
        }

        // =================================================================
        //  CREATE PROFILE
        //  Admin can create for any trainer by sending UserId.
        //  A trainer creating their own leaves UserId null.
        // =================================================================
        public async Task<GeneralResponse<TrainerProfileResponse>> CreateProfile(
            CreateTrainerProfileRequest request, int callerUserId, bool callerIsAdmin)
        {
            // a trainer can only create their own profile
            var targetUserId = callerIsAdmin ? (request.UserId ?? 0) : callerUserId;

            if (targetUserId <= 0)
                return Fail("UserId is required when the Admin creates a trainer profile");

            var user = await _trainerRepo.GetUserByIdAsync(targetUserId);

            if (user is null)
                return Fail("User not found", HttpStatusCode.NotFound);

            if (!string.Equals(user.Role, "TRAINER", StringComparison.OrdinalIgnoreCase))
                return Fail("This user is not a trainer");

            if (user.Status != "ACTIVE")
                return Fail("This account is not active");

            if (await _trainerRepo.GetByUserIdAsync(targetUserId) is not null)
                return Fail("A trainer profile already exists for this user");

            if (request.ExperienceYears < 0 || request.ExperienceYears > 60)
                return Fail("Experience years must be between 0 and 60");

            if (request.MaxClients < 1 || request.MaxClients > 100)
                return Fail("Max clients must be between 1 and 100");

            var trainer = new Trainer
            {
                // TrainerId is assigned by MySQL AUTO_INCREMENT
                UserId = targetUserId,
                Discipline = request.Discipline?.Trim(),
                Certification = request.Certification?.Trim(),
                ExperienceYears = request.ExperienceYears,
                Bio = request.Bio?.Trim(),
                MaxClients = request.MaxClients,
                JoiningDate = request.JoiningDate ?? DateOnly.FromDateTime(DateTime.Today),
                Status = "ACTIVE"
            };

            // The unique index on trainer_code is the real guard. If two requests
            // generate the same code at once, one insert fails and we try again.
            for (var attempt = 1; ; attempt++)
            {
                trainer.TrainerCode = await _trainerRepo.GenerateTrainerCodeAsync();

                try
                {
                    await _trainerRepo.AddAsync(trainer);
                    break;
                }
                catch (DbUpdateException) when (attempt < CodeRetryAttempts)
                {
                    // another request took this code, generate the next one
                }
            }

            return Ok(Map(trainer, user), "Trainer profile created successfully");
        }

        // =================================================================
        public async Task<GeneralResponse<TrainerProfileResponse>> GetProfile(int userId)
        {
            var trainer = await _trainerRepo.GetByUserIdAsync(userId);

            if (trainer is null)
                return Fail("Trainer profile not found", HttpStatusCode.NotFound);

            var user = await _trainerRepo.GetUserByIdAsync(userId);

            if (user is null)
                return Fail("User not found", HttpStatusCode.NotFound);

            return Ok(Map(trainer, user), "Trainer profile");
        }

        // =================================================================
        public async Task<GeneralResponse<List<TrainerProfileResponse>>> GetAll()
        {
            var trainers = await _trainerRepo.GetAllAsync();
            var list = new List<TrainerProfileResponse>();

            foreach (var trainer in trainers)
            {
                var user = await _trainerRepo.GetUserByIdAsync(trainer.UserId);
                if (user is not null) list.Add(Map(trainer, user));
            }

            return new GeneralResponse<List<TrainerProfileResponse>>
            {
                Success = true,
                Message = $"{list.Count} trainers",
                HttpStatusCode = HttpStatusCode.OK,
                Data = list
            };
        }

        // =================================================================
        public async Task<GeneralResponse<TrainerProfileResponse>> UpdateProfile(
            UpdateTrainerProfileRequest request, int targetUserId, bool callerIsAdmin)
        {
            var trainer = await _trainerRepo.GetByUserIdAsync(targetUserId);

            if (trainer is null)
                return Fail("Trainer profile not found", HttpStatusCode.NotFound);

            var user = await _trainerRepo.GetUserByIdAsync(targetUserId);

            if (user is null)
                return Fail("User not found", HttpStatusCode.NotFound);

            if (request.ExperienceYears < 0 || request.ExperienceYears > 60)
                return Fail("Experience years must be between 0 and 60");

            if (request.MaxClients < 1 || request.MaxClients > 100)
                return Fail("Max clients must be between 1 and 100");

            trainer.Discipline = request.Discipline?.Trim();
            trainer.Certification = request.Certification?.Trim();
            trainer.ExperienceYears = request.ExperienceYears;
            trainer.Bio = request.Bio?.Trim();
            trainer.MaxClients = request.MaxClients;

            // only the Admin may deactivate a trainer
            if (callerIsAdmin && !string.IsNullOrWhiteSpace(request.Status))
                trainer.Status = request.Status.Trim().ToUpperInvariant();

            await _trainerRepo.SaveChangesAsync();

            return Ok(Map(trainer, user), "Trainer profile updated successfully");
        }

        // -----------------------------------------------------------------
        private static TrainerProfileResponse Map(Trainer t, User u) => new()
        {
            TrainerId = t.TrainerId,
            UserId = t.UserId,
            TrainerCode = t.TrainerCode,
            Name = u.Name,
            Email = u.Email ?? string.Empty,
            Phone = u.Phone ?? string.Empty,
            Discipline = t.Discipline,
            Certification = t.Certification,
            ExperienceYears = t.ExperienceYears,
            Bio = t.Bio,
            MaxClients = t.MaxClients,
            JoiningDate = t.JoiningDate,
            Status = t.Status
        };

        private static GeneralResponse<TrainerProfileResponse> Ok(
            TrainerProfileResponse data, string message) => new()
            {
                Success = true,
                Message = message,
                HttpStatusCode = HttpStatusCode.OK,
                Data = data
            };

        private static GeneralResponse<TrainerProfileResponse> Fail(
            string message, HttpStatusCode code = HttpStatusCode.BadRequest) => new()
            {
                Success = false,
                Message = message,
                HttpStatusCode = code,
                Data = null
            };
    }
}
