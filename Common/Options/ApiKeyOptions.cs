using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Options
{
    public sealed class ApiKeyOptions
    {
        public const string Section = "ApiKey";

        public string HeaderName { get; set; } = "X-API-KEY";
        public string Key { get; set; } = string.Empty;
    }
}
