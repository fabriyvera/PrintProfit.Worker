using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace PrintProfit.Worker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


public sealed class PrintIngestionService : BackgroundService
{
    private readonly PrintServiceOptions _options;
    private readonly SqlPrintRepository _repository;
    private readonly ILogger<PrintIngestionService> _logger;

    public PrintIngestionService(
        IOptions<PrintServiceOptions> options,
        SqlPrintRepository repository,
        ILogger<PrintIngestionService> logger)
    {
        _options = options.Value;
        _repository = repository;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("PrintProfit iniciado. LogName={LogName}", _options.LogName);

        await _repository.InsertPrintJobAsync(new PrintJobInsertDto
        {
            SourceEventRecordId = 999001,
            SourceJobId = 1,
            QueueName = "Epson_L14150_BN",
            DocumentName = "Prueba desde Worker",
            SubmittedBy = Environment.UserName,
            TotalPages = 2,
            TotalBytes = 1024,
            JobStatus = "COMPLETED",
            SubmittedAt = DateTime.Now,
            CompletedAt = DateTime.Now
        }, stoppingToken);

        _logger.LogInformation("Inserción de prueba enviada a SQL.");

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}