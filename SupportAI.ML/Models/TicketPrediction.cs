using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportAI.ML.Models
{
    public record TicketPrediction
    {
        [ColumnName("PredictedLabel")]
        public string? PredictedCategory { get; init; }
    }
}
