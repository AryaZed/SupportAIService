using SupportAI.Shared.DTOs;

namespace SupportAI.Application.Interfaces.Services
{
    public interface ISupportTicketService
    {
        Task<List<SupportTicketDto>> GetTicketsAsync();
        Task<SupportTicketDto> CreateTicketAsync(SupportTicketDto ticket);
    }

}
