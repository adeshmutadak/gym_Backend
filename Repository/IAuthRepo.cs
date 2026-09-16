using System;
using System.Collections.Generic;
using System.Text;
using gym_application.Models;

namespace Repository
{
    public interface IAuthRepo
    {
        Task<User?> GetUserByIdAsync(int userId);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByMobileAsync(string mobile);
        Task<User?> GetUserByEmailOrMobAsync(string emailOrMobile);
        Task AddUserAsync(User user);
        Task SaveChangesAsync();

    }
}
