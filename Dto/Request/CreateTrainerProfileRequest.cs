using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto.Request
{
    public class CreateTrainerProfileRequest
    {
        /// <summary>Only the Admin sets this. A trainer creating their own profile leaves it null.</summary>
        public int? UserId { get; set; }

        public string? Discipline { get; set; }
        public string? Certification { get; set; }
        public int ExperienceYears { get; set; }
        public string? Bio { get; set; }
        public int MaxClients { get; set; } = 8;
        public DateOnly? JoiningDate { get; set; }
        // trainer_id and trainer_code are generated, never supplied
    }

    public class UpdateTrainerProfileRequest
    {
        public string? Discipline { get; set; }
        public string? Certification { get; set; }
        public int ExperienceYears { get; set; }
        public string? Bio { get; set; }
        public int MaxClients { get; set; }
        public string? Status { get; set; }          // Admin only
    }
}
