using MediatR;
using SupportAI.Application.Interfaces.Repo;
using SupportAI.Shared.DTOs;

namespace SupportAI.Application.Handlers.Tickets
{
    public class GetTicketsHandler(ISupportTicketRepository repo) : IRequestHandler<GetTicketsQuery, List<SupportTicketDto>>
    {
        public async Task<List<SupportTicketDto>> Handle(GetTicketsQuery request, CancellationToken cancellationToken) =>
            (await repo.GetAllAsync()).Select(t => new SupportTicketDto(t.Id, t.Title, t.Description, t.Status)).ToList();

    }
}
