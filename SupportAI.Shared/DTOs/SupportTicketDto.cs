using SupportAI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportAI.Shared.DTOs
{
    public record SupportTicketDto(Guid Id, string Title, string Description, TicketStatus Status);
}
