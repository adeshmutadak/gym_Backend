using System;
using System.Collections.Generic;
using System.Text;

namespace Dto.Response
{
    public class RegistrationResponse
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        /// <summary>Share this with the customer, for example Adii@123.</summary>
        public string DefaultPassword { get; set; } = string.Empty;

        public bool IsPasswordReset { get; set; }               // false at creation
    }
}
