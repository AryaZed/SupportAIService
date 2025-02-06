using Microsoft.AspNetCore.SignalR;

namespace SupportAI.Shared.Hubs;

public class TicketHub : Hub
{
    public async Task NotifyNewTicket(string ticketId, string title)
    {
        await Clients.All.SendAsync("ReceiveNewTicket", ticketId, title);
    }

    public async Task NotifyTicketUpdate(string ticketId, string status)
    {
        await Clients.All.SendAsync("ReceiveTicketUpdate", ticketId, status);
    }
}
