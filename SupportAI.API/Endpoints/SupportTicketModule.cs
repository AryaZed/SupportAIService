using Carter;
using MediatR;
using SupportAI.Application.Commands;
using SupportAI.Application.Handlers.Tickets;

namespace SupportAI.API.Endpoints
{
    public class SupportTicketModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/tickets").RequireAuthorization();

            group.MapGet("/", async (IMediator mediator) =>
                await mediator.Send(new GetTicketsQuery()));

            group.MapPost("/", async (CreateTicketCommand command, IMediator mediator) =>
                await mediator.Send(command));
        }
    }
}
