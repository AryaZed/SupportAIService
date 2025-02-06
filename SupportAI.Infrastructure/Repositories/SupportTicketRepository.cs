using Microsoft.EntityFrameworkCore;
using SupportAI.Application.Interfaces.Repo;
using SupportAI.Domain.Entities.Ticket;
using SupportAI.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportAI.Infrastructure.Repositories
{
    public class SupportTicketRepository : ISupportTicketRepository
    {
        private readonly AppDbContext _context;

        public SupportTicketRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SupportTicket>> GetAllAsync()
        {
            return await _context.SupportTickets.ToListAsync();
        }

        public async Task<SupportTicket> GetByIdAsync(Guid id) 
            => await _context.SupportTickets.FindAsync(id);

        public async Task AddAsync(SupportTicket ticket)
        {
            await _context.SupportTickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }
    }
}
