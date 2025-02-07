using Microsoft.ML.Data;

namespace SupportAI.ML.Models
{
    public record SentimentData
    {
        [LoadColumn(0)]
        public required string Text { get; init; }

        [LoadColumn(1)]
        public bool Sentiment { get; init; }
    }
}
