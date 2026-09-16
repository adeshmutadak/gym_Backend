using System;
using System.Collections.Generic;
using System.Text;

namespace Dto.Request
{
    /// <summary>API 3 - Customer or trainer login.</summary>
    public class LogRequest
    {
        public string EmailOrMobile { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
