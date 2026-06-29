using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace PrintProfit.Worker;

public sealed class PrintIngestionService : BackgroundService
{
    private readonly PrintServiceOptions _options;
    private readonly SqlPrintRepository _repository;
    private readonly ILogger<PrintIngestionService> _logger;
    private readonly ILogger<PrintOperationalLogReader> _logReaderLogger;

    public PrintIngestionService(
        IOptions<PrintServiceOptions> options,
        SqlPrintRepository repository,
        ILogger<PrintIngestionService> logger,
        ILogger<PrintOperationalLogReader> logReaderLogger)
    {
        _options = options.Value;
        _repository = repository;
        _logger = logger;
        _logReaderLogger = logReaderLogger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("PrintProfit iniciado. LogName={LogName}", _options.LogName);

        using var reader = new PrintOperationalLogReader(
            _options.LogName,
            _logReaderLogger,
            dto => _repository.InsertPrintJobAsync(dto, stoppingToken));

        reader.Start();

        try
        {
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            // Cierre normal al detener el servicio.
        }
    }
}