using MediatR;
using SupportAI.Shared.DTOs;

namespace SupportAI.Application.Commands;

public record CreateTicketCommand(string Title, string Description) : IRequest<SupportTicketDto>;
