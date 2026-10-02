using Microsoft.Extensions.Hosting;
using PrintProfit.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "PrintProfit Worker";
});

builder.Services.Configure<PrintServiceOptions>(
    builder.Configuration.GetSection("PrintService"));

builder.Services.AddSingleton<SqlPrintRepository>();
builder.Services.AddHostedService<PrintIngestionService>();

var host = builder.Build();
host.Run();