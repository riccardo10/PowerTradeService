using Serilog;
using Quartz;

using Windows.Service.PowerTradeService; 
using PowerTradeService.Jobs;
using Services;

var hostBuilder = Host.CreateApplicationBuilder(args);

//hostBuilder.Services.AddHostedService<Windows.Service.PowerTradeService.PowerTradeService>();

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.Debug()
            .WriteTo.File(path: Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs/powertrades.log"), rollingInterval: RollingInterval.Day)
            .CreateLogger();

hostBuilder.Services.AddSerilog(logger: Log.Logger, dispose: true);

hostBuilder.Services.AddTransient<PowerTradeJob>();
hostBuilder.Services.AddTransient<IPowerService, PowerService>();

hostBuilder.Services.AddQuartz(static q =>
{
    JobKey jobKey = new JobKey("PowerTradeJob", "PowerTradeGroup");
    q.AddJob<PowerTradeJob>(configure => configure.WithIdentity(jobKey));
    
    q.AddTrigger(configure => configure
                .ForJob(jobKey)
                .WithIdentity("PowerTradeJobTrigger", "PowerTradeGroup")
                .StartNow()
                .EndAt(null)
                );
});

hostBuilder.Services.AddQuartzHostedService(static q => 
{
    q.WaitForJobsToComplete = true; // Wait for jobs to complete before shutting down
});

hostBuilder.Services.AddWindowsService(static configure =>
{
    configure.ServiceName = "Power Trade .Net Windows Service v1.0.0";
});

IHost? host = hostBuilder.Build();

host.Run();
