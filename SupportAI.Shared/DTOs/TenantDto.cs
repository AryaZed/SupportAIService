using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportAI.Shared.DTOs
{
    public record TenantDto(Guid Id, string Name, string Domain);
}
