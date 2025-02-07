using Microsoft.ML.Data;

namespace SupportAI.ML.Models
{
    public record SentimentPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool Prediction { get; init; }

        public float Probability { get; init; }

        public float Score { get; init; }
    }
}
