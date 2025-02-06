using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportAI.Shared.Configuration
{
    public class JwtSettings
    {
        public required string SecretKey { get; set; }
        public bool ValidateIssuer { get; set; } = false;
        public bool ValidateAudience { get; set; } = false;
        public bool ValidateLifetime { get; set; } = true;
    }
}
