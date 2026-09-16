using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gym_application.Models;
using Microsoft.EntityFrameworkCore;
namespace Repository
{
    public class TrainerRepo : ITrainerRepo
    {
        private readonly GymDbContext _context;

        public TrainerRepo(GymDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<User?> GetUserByIdAsync(int userId)
            => await _context.Users.FirstOrDefaultAsync(x => x.UserId == userId);

        public async Task<Trainer?> GetByUserIdAsync(int userId)
            => await _context.Trainers.FirstOrDefaultAsync(x => x.UserId == userId);

        public async Task<Trainer?> GetByTrainerIdAsync(int trainerId)
            => await _context.Trainers.FirstOrDefaultAsync(x => x.TrainerId == trainerId);

        public async Task<List<Trainer>> GetAllAsync()
            => await _context.Trainers.OrderBy(x => x.TrainerCode).ToListAsync();

        /// <summary>Returns the next code in the TR01, TR02 sequence.</summary>
        public async Task<string> GenerateTrainerCodeAsync()
        {
            var codes = await _context.Trainers
                .Where(x => x.TrainerCode.StartsWith("TR"))
                .Select(x => x.TrainerCode)
                .ToListAsync();

            var highest = codes
                .Select(c => int.TryParse(c.Substring(2), out var n) ? n : 0)
                .DefaultIfEmpty(0)
                .Max();

            return $"TR{highest + 1:D2}";     // TR01 ... TR99, then TR100
        }

        public async Task AddAsync(Trainer trainer)
        {
            _context.Trainers.Add(trainer);
            await _context.SaveChangesAsync();   // TrainerId is populated here
        }

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
