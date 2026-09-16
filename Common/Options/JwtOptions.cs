using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Options
{
    public sealed class JwtOptions
    {
        public const string Section = "Jwt";
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int AccessTokenMinutes { get; set; } = 120;
        public string SigningKey { get; set; } = string.Empty;
    }
}
