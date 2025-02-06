using Microsoft.EntityFrameworkCore;
using SupportAI.Domain.Entities;
using SupportAI.Domain.Entities.Identity;
using SupportAI.Domain.Entities.Ticket;

namespace SupportAI.Infrastructure.Persistence
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<SupportTicket> SupportTickets { get; set; }
    }
}
