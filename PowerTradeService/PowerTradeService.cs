using System.Threading;

namespace Windows.Service.PowerTradeService
{
    public class PowerTradeService : BackgroundService
    {
        private readonly ILogger<PowerTradeService>? _logger;

        public PowerTradeService(ILogger<PowerTradeService>? logger)
        {
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
            /*
            while (!cancellationToken.IsCancellationRequested)
            {
                if (_logger!.IsEnabled(LogLevel.Information))
                {
                   _logger?.LogInformation($"Power Trade (.Net) Service is running at {TimeOnly.FromDateTime(DateTime.UtcNow).ToString("HH:mm:ss")}.");
                    await Task.Delay(5000, cancellationToken);
                }
            }
   
            _logger?.LogInformation("Power Trade (.Net) Service is stopping.");
            */
            
        }
    }
}
