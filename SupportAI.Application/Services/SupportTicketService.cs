using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using SupportAI.Application.Interfaces.Repo;
using SupportAI.Application.Interfaces.Services;
using SupportAI.Domain.Entities.Ticket;
using SupportAI.Domain.Enums;
using SupportAI.ML.Services;
using SupportAI.Shared.DTOs;
using SupportAI.Shared.Hubs;

namespace SupportAI.Application.Services;

public class SupportTicketService(ISupportTicketRepository _ticketRepository, IHttpContextAccessor _httpContextAccessor,
    TicketAIService _aiService, IHubContext<TicketHub> _hubContext)
    : ISupportTicketService
{
    public async Task<List<SupportTicketDto>> GetTicketsAsync()
    {
        var tickets = await _ticketRepository.GetAllAsync();
        return tickets.Select(t => new SupportTicketDto(t.Id, t.Title, t.Description, t.Status)).ToList();
    }

    public async Task<SupportTicketDto> CreateTicketAsync(SupportTicketDto ticket)
    {
        var tenantIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("TenantId")?.Value;
        if (string.IsNullOrEmpty(tenantIdClaim))
        {
            throw new UnauthorizedAccessException("TenantId is missing in user context.");
        }

        // AI Prediction for Ticket Categorization
        var predictedCategory = _aiService.PredictCategory(ticket.Description);

        var entity = new SupportTicket
        {
            Id = Guid.NewGuid(),
            TenantId = Guid.Parse(tenantIdClaim),
            Title = ticket.Title,
            Description = ticket.Description,
            Status = TicketStatus.Open,
            Category = predictedCategory
        };

        await _ticketRepository.AddAsync(entity);

        await _hubContext.Clients.All.SendAsync("ReceiveNewTicket", entity.Id.ToString(), entity.Title);

        return ticket;
    }
}
