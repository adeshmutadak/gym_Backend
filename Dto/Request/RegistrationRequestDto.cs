using System;
using System.Collections.Generic;
using System.Text;

namespace Dto.Request
{
    /// <summary>API 2 - Admin adds a customer or trainer.</summary>
    public class RegistrationRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;      // CLIENT or TRAINER
        public string? Gender { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        // no Password - the API sets Name@123
    }

}
