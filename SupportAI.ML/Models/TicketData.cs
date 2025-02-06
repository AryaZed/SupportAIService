using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportAI.ML.Models
{
    public record TicketData
    {
        [LoadColumn(0)] public required string IssueDescription { get; init; }
        [LoadColumn(1)] public required string Category { get; init; } // Training Label
    }
}
