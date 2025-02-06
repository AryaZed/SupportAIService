using Carter;
using SupportAI.ML.Services;

namespace SupportAI.API.Endpoints;

public class AIModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/ai");

        group.MapPost("/predict-category", (string description, TicketAIService aiService) =>
        {
            var category = aiService.PredictCategory(description);
            return Results.Ok(new { Category = category });
        });
    }
}
