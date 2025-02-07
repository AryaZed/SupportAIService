using Microsoft.AspNetCore.Identity;

namespace SupportAI.Domain.Entities.Identity
{
    // Inherit from IdentityUser<Guid> for ASP.NET Core Identity
    public class User : IdentityUser<Guid>
    {
        public Guid TenantId { get; set; } // Each user belongs to a specific tenant
        public string FullName { get; set; } = default!;
    }
}
