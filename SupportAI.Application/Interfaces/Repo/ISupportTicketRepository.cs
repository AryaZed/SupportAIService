using SupportAI.Domain.Entities.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportAI.Application.Interfaces.Repo
{
    public interface ISupportTicketRepository
    {
        Task<List<SupportTicket>> GetAllAsync();
        Task<SupportTicket> GetByIdAsync(Guid id);
        Task AddAsync(SupportTicket ticket);
    }
}
