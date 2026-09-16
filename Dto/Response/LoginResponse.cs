using System;
using System.Collections.Generic;
using System.Text;

namespace Dto.Response
{
    public class LoginResponse
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;        // Admin / Trainer / Client

        /// <summary>False while the user is still on the default password.</summary>
        public bool IsPasswordReset { get; set; }

        public string Token { get; set; } = string.Empty;
    }
}
