using System.Diagnostics.Eventing.Reader;
using Microsoft.Extensions.Logging;

namespace PrintProfit.Worker;

public sealed class PrintOperationalLogReader : IDisposable
{
    private readonly EventLogWatcher _watcher;
    private readonly ILogger<PrintOperationalLogReader> _logger;
    private readonly Func<PrintJobInsertDto, Task> _onJobCompletedAsync;

    public PrintOperationalLogReader(
        string logName,
        ILogger<PrintOperationalLogReader> logger,
        Func<PrintJobInsertDto, Task> onJobCompletedAsync)
    {
        _logger = logger;
        _onJobCompletedAsync = onJobCompletedAsync;

        var query = new EventLogQuery(
            logName,
            PathType.LogName,
            "*[System/EventID=307]");

        _watcher = new EventLogWatcher(query);
        _watcher.EventRecordWritten += OnEventRecordWritten;
    }

    public void Start()
    {
        _watcher.Enabled = true;
        _logger.LogInformation("Escuchando impresiones (Event ID 307)...");
    }

    private void OnEventRecordWritten(object? sender, EventRecordWrittenEventArgs e)
    {
        if (e.EventRecord is null) return;

        try
        {
            using var record = e.EventRecord;
            var dto = MapEventToDto(record);

            if (dto is null)
            {
                _logger.LogWarning(
                    "Evento 307 ignorado (datos incompletos). RecordId={RecordId}",
                    record.RecordId);
                return;
            }

            _ = Task.Run(async () =>
            {
                try
                {
                    await _onJobCompletedAsync(dto);
                    _logger.LogInformation(
                        "Trabajo registrado: Queue={Queue}, Pages={Pages}, Doc={Doc}",
                        dto.QueueName, dto.TotalPages, dto.DocumentName);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error guardando trabajo en SQL. RecordId={RecordId}",
                        dto.SourceEventRecordId);
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error procesando evento de impresión.");
        }
    }

    private static PrintJobInsertDto? MapEventToDto(EventRecord record)
    {
        if (record.Properties.Count < 5) return null;

        var documentName = record.Properties[0].Value?.ToString();
        var submittedBy = record.Properties[1].Value?.ToString();
        var queueName = record.Properties[2].Value?.ToString();

        long? totalBytes = long.TryParse(record.Properties[3].Value?.ToString(), out var bytes)
            ? bytes
            : null;

        int? totalPages = int.TryParse(record.Properties[4].Value?.ToString(), out var pages)
            ? pages
            : null;

        if (string.IsNullOrWhiteSpace(queueName)) return null;

        var eventTime = record.TimeCreated ?? DateTime.Now;

        return new PrintJobInsertDto
        {
            SourceEventRecordId = record.RecordId,
            SourceJobId = null,
            QueueName = queueName,
            DocumentName = documentName,
            SubmittedBy = submittedBy,
            TotalPages = totalPages,
            TotalBytes = totalBytes,
            JobStatus = "COMPLETED",
            SubmittedAt = eventTime,
            CompletedAt = eventTime
        };
    }

    public void Dispose()
    {
        _watcher.Dispose();
    }
}