using Carter;
using Microsoft.AspNetCore.Http;
using SupportAI.ML.Services;

namespace SupportAI.API.Endpoints
{
    public class SentimentAnalysisModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            // Map an endpoint to perform sentiment prediction.
            app.MapPost("/api/sentiment", (SentimentRequest request, SentimentAnalysisService sentimentService) =>
            {
                var prediction = sentimentService.PredictSentiment(request.Text);
                return Results.Ok(new
                {
                    Prediction = prediction.Prediction,
                    Probability = prediction.Probability,
                    Score = prediction.Score
                });
            });
        }
    }

    public record SentimentRequest(string Text);
}
