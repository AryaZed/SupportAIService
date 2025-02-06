using SupportAI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportAI.Domain.Entities.Ticket;

public class SupportTicket
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required Guid TenantId { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public TicketStatus Status { get; set; } = TicketStatus.Open;
    public string Category { get; set; }
}

