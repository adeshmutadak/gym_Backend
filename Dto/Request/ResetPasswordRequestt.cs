using System;
using System.Collections.Generic;
using System.Text;

namespace Dto.Request
{
    /// <summary>API 4 - the signed-in user sets their own password.</summary>
    public class ResetPasswordRequestt
    {
        public int UserId { get; set; }
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
