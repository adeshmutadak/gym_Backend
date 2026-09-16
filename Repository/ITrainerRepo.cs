using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gym_application.Models;

namespace Repository
{
    public interface ITrainerRepo
    {
        Task<User?> GetUserByIdAsync(int userId);
        Task<Trainer?> GetByUserIdAsync(int userId);
        Task<Trainer?> GetByTrainerIdAsync(int trainerId);
        Task<List<Trainer>> GetAllAsync();
        Task<string> GenerateTrainerCodeAsync();
        Task AddAsync(Trainer trainer);
        Task SaveChangesAsync();
    }
}
