using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SupportAI.Domain.Entities.Identity;
using SupportAI.Domain.Entities.Ticket;
using SupportAI.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SupportAI.Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<SupportTicket> SupportTickets { get; set; }
    }
}
