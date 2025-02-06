using Microsoft.ML;
using SupportAI.ML.Models;

namespace SupportAI.ML.Services;

public class TicketAIService
{
    private readonly MLContext _mlContext = new();
    private readonly ITransformer _model;

    public TicketAIService()
    {
        _model = _mlContext.Model.Load("SupportAI.ML/Models/TicketCategorizationModel.zip", out _);
    }

    public string PredictCategory(string description)
    {
        var predictor = _mlContext.Model.CreatePredictionEngine<TicketData, TicketPrediction>(_model);
        var prediction = predictor.Predict(new TicketData { IssueDescription = description, Category = "" });
        return prediction.PredictedCategory ?? "Unknown";
    }
}
