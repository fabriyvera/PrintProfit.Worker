using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintProfit.Worker;

public sealed class PrintJobInsertDto
{
    public long? SourceEventRecordId { get; init; }
    public int? SourceJobId { get; init; }
    public required string QueueName { get; init; }
    public string? DocumentName { get; init; }
    public string? SubmittedBy { get; init; }
    public int? TotalPages { get; init; }
    public long? TotalBytes { get; init; }
    public required string JobStatus { get; init; }
    public DateTime SubmittedAt { get; init; }
    public DateTime? CompletedAt { get; init; }
}
