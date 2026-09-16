using System;
using System.Collections.Generic;
using System.Text;

namespace Dto.Request
{
    public class ResendCredentialsRequest
    {
        public string EmailOrMobile { get; set; } = string.Empty;
    }
}
