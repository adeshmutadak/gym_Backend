using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLayer.CommonResponse;
using Dto.Request;
using Dto.Response;

namespace Services
{
    public interface ITrainerService
    {
        Task<GeneralResponse<TrainerProfileResponse>> CreateProfile(
            CreateTrainerProfileRequest request, int callerUserId, bool callerIsAdmin);

        Task<GeneralResponse<TrainerProfileResponse>> GetProfile(int userId);

        Task<GeneralResponse<List<TrainerProfileResponse>>> GetAll();

        Task<GeneralResponse<TrainerProfileResponse>> UpdateProfile(
            UpdateTrainerProfileRequest request, int targetUserId, bool callerIsAdmin);
    }
}
