using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Options
{
    public sealed class AdminAccountOptions
    {
        public const string Section = "AdminAccount";
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;   // plain text, compared directly
        public string Role { get; set; } = "Admin";
    }
}
