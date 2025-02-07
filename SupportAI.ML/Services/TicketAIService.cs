using Microsoft.ML;
using SupportAI.ML.Models;
using SupportAI.ML.Utils;

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
        var normalizedText = NormalizePersianText(description);
        var cleanedText = RemovePersianStopWords(normalizedText);

        var predictor = _mlContext.Model.CreatePredictionEngine<TicketData, TicketPrediction>(_model);
        var prediction = predictor.Predict(new TicketData { IssueDescription = cleanedText, Category = "" });

        return prediction.PredictedCategory ?? "نامشخص"; // Default "Unknown" in Persian
    }

    private string NormalizePersianText(string text)
    {
        return text.Replace("ي", "ی") // Arabic Yeh to Persian Yeh
                   .Replace("ك", "ک") // Arabic Keheh to Persian Keheh
                   .ToLower();
    }

    private string RemovePersianStopWords(string text)
    {
        var stopWords = PersianStopWords.StopWords;
        var words = text.Split(' ').Where(word => !stopWords.Contains(word));
        return string.Join(" ", words);
    }
}
