using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Common.Options;
using CommonLayer.CommonResponse;
using CommonLayer.SecurityHelper;
using Dto.Request;
using Dto.Response;
using gym_application.Models;
using Microsoft.Extensions.Options;
using Repository;

namespace Services
{
    public class AuthService : IAuthService
    {
        private const int NotReset = 0;   // still on the default password
        private const int HasReset = 1;   // the user set their own password

        private static readonly string[] AllowedRoles = { "TRAINER", "CLIENT" };

        private readonly IAuthRepo _authRepo;
        private readonly ISecurityHelper _securityHelper;
        private readonly AdminAccountOptions _admin;

        public AuthService(IAuthRepo authRepo,
                           ISecurityHelper securityHelper,
                           IOptions<AdminAccountOptions> admin)
        {
            _authRepo = authRepo;
            _securityHelper = securityHelper;
            _admin = admin.Value;
        }

        // =================================================================
        //  API 1 · ADMIN LOGIN
        //  Credentials come from appsettings. No hashing, no database call.
        // =================================================================
        public GeneralResponse<LoginResponse> AdminLogin(AdminLoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return Fail<LoginResponse>("Username and password are required");

            var ok = string.Equals(request.Username, _admin.Username, StringComparison.Ordinal)
                     && string.Equals(request.Password, _admin.Password, StringComparison.Ordinal);

            if (!ok)
                return Fail<LoginResponse>("Invalid admin credentials", HttpStatusCode.Unauthorized);

            return Ok(new LoginResponse
            {
                UserId = 0,
                Name = "Administrator",
                Role = _admin.Role,                 // "Admin"
                IsPasswordReset = true,
                Token = _securityHelper.GenerateJwtToken(0, _admin.Username, _admin.Role)
            }, "Admin login successful");
        }

        // =================================================================
        //  API 2 · ADMIN ADDS A CUSTOMER OR TRAINER
        //  Password becomes Name@123. must_reset_password = 0.
        // =================================================================
        public async Task<GeneralResponse<RegistrationResponse>> AddUser(RegistrationRequestDto request)
        {
            var role = request.Role?.Trim().ToUpperInvariant() ?? string.Empty;

            if (!AllowedRoles.Contains(role))
                return Fail<RegistrationResponse>("Role must be either CLIENT or TRAINER");

            if (string.IsNullOrWhiteSpace(request.Name))
                return Fail<RegistrationResponse>("Name is required");

            if (string.IsNullOrWhiteSpace(request.Email) && string.IsNullOrWhiteSpace(request.Phone))
                return Fail<RegistrationResponse>("An email address or a mobile number is required");

            if (!string.IsNullOrWhiteSpace(request.Phone)
                && await _authRepo.GetUserByMobileAsync(request.Phone.Trim()) is not null)
                return Fail<RegistrationResponse>("A user already exists with this number");

            if (!string.IsNullOrWhiteSpace(request.Email)
                && await _authRepo.GetUserByEmailAsync(request.Email.Trim()) is not null)
                return Fail<RegistrationResponse>("A user already exists with this email");

            var defaultPassword = _securityHelper.GenerateDefaultPassword(request.Name);   // Adii@123

            var user = new User
            {
                // UserId is assigned by MySQL AUTO_INCREMENT
                Role = role,
                Name = request.Name.Trim(),
                Email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email.Trim(),
                Phone = string.IsNullOrWhiteSpace(request.Phone) ? null : request.Phone.Trim(),
                Gender = string.IsNullOrWhiteSpace(request.Gender) ? "UNDISCLOSED" : request.Gender.Trim(),
                DateOfBirth = request.DateOfBirth,
                PasswordHash = _securityHelper.HashPassword(defaultPassword),
                MustResetPassword = NotReset,        // 0
                Status = "ACTIVE",
                CreatedAt = DateTime.UtcNow
            };

            await _authRepo.AddUserAsync(user);

            return Ok(new RegistrationResponse
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email ?? string.Empty,
                Phone = user.Phone ?? string.Empty,
                Role = role,
                DefaultPassword = defaultPassword,
                IsPasswordReset = false
            }, "User added successfully. Share the default password with the user.");
        }

        // =================================================================
        //  API 3 · CUSTOMER OR TRAINER LOGIN
        //  Works with the default password as well as a reset one.
        // =================================================================
        public async Task<GeneralResponse<LoginResponse>> UserLogin(LogRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.EmailOrMobile) || string.IsNullOrWhiteSpace(request.Password))
                return Fail<LoginResponse>("Email or mobile number and password are required");

            var user = await _authRepo.GetUserByEmailOrMobAsync(request.EmailOrMobile.Trim());

            if (user is null || !_securityHelper.VerifyPassword(request.Password, user.PasswordHash))
                return Fail<LoginResponse>("Invalid email or mobile number, or password",
                                           HttpStatusCode.Unauthorized);

            var role = user.Role == "TRAINER" ? "Trainer" : "Client";
            var isPasswordReset = user.MustResetPassword == HasReset;

            return Ok(new LoginResponse
            {
                UserId = user.UserId,
                Name = user.Name,
                Role = role,
                IsPasswordReset = isPasswordReset,
                Token = _securityHelper.GenerateJwtToken(
                            user.UserId,
                            user.Email ?? user.Phone ?? string.Empty,
                            role)
            }, isPasswordReset
                 ? "Login successful"
                 : "Login successful. You are still using the default password.");
        }

        // -----------------------------------------------------------------
        private static GeneralResponse<T> Ok<T>(T data, string message) => new()
        {
            Success = true,
            Message = message,
            HttpStatusCode = HttpStatusCode.OK,
            Data = data
        };

        private static GeneralResponse<T> Fail<T>(string message,
            HttpStatusCode code = HttpStatusCode.BadRequest) => new()
            {
                Success = false,
                Message = message,
                HttpStatusCode = code,
                Data = default
            };


        // =================================================================
        //  API 4 · RESET PASSWORD
        //  The user is identified from their token, not from the body.
        //  On success must_reset_password becomes 1.
        // =================================================================
        public async Task<BaseResponse> ResetPassword(int userId, ResetPasswordRequestt request)
        {
            // the Admin password lives in appsettings, not in the database
            if (userId == 0)
                return FailBase("The admin password is changed in configuration, not through this endpoint");

            if (string.IsNullOrWhiteSpace(request.CurrentPassword))
                return FailBase("Current password is required");

            if (string.IsNullOrWhiteSpace(request.NewPassword))
                return FailBase("New password is required");

            if (!string.Equals(request.NewPassword, request.ConfirmPassword, StringComparison.Ordinal))
                return FailBase("The new password and confirmation do not match");

            var user = await _authRepo.GetUserByIdAsync(userId);

            if (user is null)
                return FailBase("User not found", HttpStatusCode.NotFound);

            if (user.Status != "ACTIVE")
                return FailBase("This account is not active. Please contact the gym.");

            if (!_securityHelper.VerifyPassword(request.CurrentPassword, user.PasswordHash))
                return FailBase("Current password is incorrect", HttpStatusCode.Unauthorized);

            user.PasswordHash = _securityHelper.HashPassword(request.NewPassword);
            user.MustResetPassword = HasReset;          // 1
            await _authRepo.SaveChangesAsync();

            return new BaseResponse
            {
                Success = true,
                Message = "Password reset successfully",
                HttpStatusCode = HttpStatusCode.OK
            };
        }

        // helper, next to the existing Ok and Fail methods
        private static BaseResponse FailBase(string message,
            HttpStatusCode code = HttpStatusCode.BadRequest) => new()
            {
                Success = false,
                Message = message,
                HttpStatusCode = code
            };
    }
}
