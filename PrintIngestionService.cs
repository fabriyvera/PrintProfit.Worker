using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace PrintProfit.Worker;

public sealed class PrintIngestionService : BackgroundService
{
    private readonly PrintServiceOptions _options;

    public PrintIngestionService(IOptions<PrintServiceOptions> options)
    {
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // TODO: EventLogReader sobre _options.LogName
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}