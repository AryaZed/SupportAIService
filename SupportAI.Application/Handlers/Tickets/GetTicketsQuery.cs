using MediatR;
using SupportAI.Shared.DTOs;

namespace SupportAI.Application.Handlers.Tickets
{
    public record GetTicketsQuery : IRequest<List<SupportTicketDto>>;

}
