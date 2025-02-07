using Microsoft.ML;
using SupportAI.ML.Models;

namespace SupportAI.ML.Services
{
    public class SentimentAnalysisService
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer _model;
        private readonly PredictionEngine<SentimentData, SentimentPrediction> _predictionEngine;

        public SentimentAnalysisService(string modelPath)
        {
            _mlContext = new MLContext();
            _model = _mlContext.Model.Load(modelPath, out _);
            _predictionEngine = _mlContext.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(_model);
        }

        public SentimentPrediction PredictSentiment(string text)
        {
            // Optionally, apply normalization or custom text processing if needed
            var input = new SentimentData { Text = text };
            return _predictionEngine.Predict(input);
        }
    }
}
