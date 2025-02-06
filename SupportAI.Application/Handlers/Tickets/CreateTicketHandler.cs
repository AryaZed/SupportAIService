using MediatR;
using Microsoft.AspNetCore.Http;
using SupportAI.Application.Commands;
using SupportAI.Application.Interfaces.Repo;
using SupportAI.Domain.Entities.Ticket;
using SupportAI.Domain.Enums;
using SupportAI.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SupportAI.Application.Handlers.Tickets
{
    public class CreateTicketHandler(ISupportTicketRepository _repo,IHttpContextAccessor _httpContextAccessor) : IRequestHandler<CreateTicketCommand, SupportTicketDto>
    {
        public async Task<SupportTicketDto> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
        {
            var tenantIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("TenantId")?.Value;
            if (string.IsNullOrEmpty(tenantIdClaim))
            {
                throw new UnauthorizedAccessException("TenantId is missing in user context.");
            }

            var entity = new SupportTicket
            {
                Id = Guid.NewGuid(),
                TenantId = Guid.Parse(tenantIdClaim),
                Title = request.Title,
                Description = request.Description,
                Status = TicketStatus.Open
            };

            await _repo.AddAsync(entity);
            return new SupportTicketDto(entity.Id, entity.Title, entity.Description, entity.Status);
        }
    }
}
