using System;
using System.Collections.Generic;
using System.Text;
using gym_application.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class AuthRepo : IAuthRepo
    {
        private readonly GymDbContext _context;

        public AuthRepo(GymDbContext dbContext)
        {
            _context = dbContext;
        }

        /// <summary>Used by reset-password, where the id comes from the token.</summary>
        public async Task<User?> GetUserByIdAsync(int userId)
            => await _context.Users.FirstOrDefaultAsync(x => x.UserId == userId);

        public async Task<User?> GetUserByEmailAsync(string email)
            => await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

        public async Task<User?> GetUserByMobileAsync(string mobile)
            => await _context.Users.FirstOrDefaultAsync(x => x.Phone == mobile);

        public async Task<User?> GetUserByEmailOrMobAsync(string emailOrMobile)
            => await _context.Users.FirstOrDefaultAsync(x =>
                   (x.Email == emailOrMobile || x.Phone == emailOrMobile)
                   && x.Status == "ACTIVE");

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();   // UserId is populated here
        }

        public async Task SaveChangesAsync()
          => await _context.SaveChangesAsync();
    }
}
