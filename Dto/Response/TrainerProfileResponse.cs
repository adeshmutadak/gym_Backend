using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto.Response
{
    public class TrainerProfileResponse
    {
        public int TrainerId { get; set; }
        public int? UserId { get; set; }
        public string TrainerCode { get; set; } = string.Empty;

        // pulled from Users so the app does not need a second call
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        public string? Discipline { get; set; }
        public string? Certification { get; set; }
        public int? ExperienceYears { get; set; }
        public string? Bio { get; set; }
        public int? MaxClients { get; set; }
        public DateOnly? JoiningDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
