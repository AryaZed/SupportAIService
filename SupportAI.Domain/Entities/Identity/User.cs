using Microsoft.AspNet.Identity.EntityFramework;

namespace SupportAI.Domain.Entities.Identity
{
    public class User : IdentityUser
    {
        public Guid TenantId { get; set; } // Each user belongs to a specific tenant
        public string FullName { get; set; } = default!;
    }
}
