using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.CommonResponse;
using Dto.Request;
using Dto.Response;

namespace Services
{
    public interface IAuthService
    {
        GeneralResponse<LoginResponse> AdminLogin(AdminLoginRequest request);
        Task<GeneralResponse<RegistrationResponse>> AddUser(RegistrationRequestDto request);
        Task<GeneralResponse<LoginResponse>> UserLogin(LogRequest request);
        Task<BaseResponse> ResetPassword(int userId, ResetPasswordRequestt request);
    }
}
