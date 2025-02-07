using Microsoft.ML.Data;

namespace SupportAI.ML.Models;

public class ProcessedTicketData
{
    [ColumnName("IssueDescription")]
    public string IssueDescription { get; set; } = default!;

    [ColumnName("Category")]
    public string Category { get; set; } = default!;
}
