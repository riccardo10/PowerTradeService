using Quartz;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FileHelpers;

namespace PowerTradeService.Jobs
{
    internal class PowerTradeJob : IJob
    {
        private readonly IPowerService _powerService;
        private readonly ILogger<PowerTradeJob>? _logger;
        private readonly IConfiguration _configuration;

        public PowerTradeJob(IPowerService powerService, IConfiguration configuration, ILogger<PowerTradeJob>? logger)
        {
            _logger = logger;
            _powerService = powerService ?? throw new ArgumentNullException(nameof(powerService));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(_configuration));
        }
        public async Task Execute(IJobExecutionContext context)
        {
            // This method is called by the Quartz scheduler when the job is executed.
            //DateTime tradeDate = DateTime.Today.AddDays(-1);

            DateTime tradeDate = new DateTime(2025, 6, 10); // Use a fixed date for testing purposes
            IEnumerable<PowerTrade> powerTrades = await _powerService.GetTradesAsync(date: tradeDate);

            if (powerTrades.Any())
            {
                _logger?.LogInformation($"Power Trade (.Net) Service is now running at {TimeOnly.FromDateTime(DateTime.Now).ToString("HH:mm:ss")}.");
                var tradePeriods = powerTrades.SelectMany(p => p.Periods).Select(v => new PowerPeriod{ Period = v.Period, Volume = v.Volume});
                var tradeVolumes = tradePeriods.GroupBy(p => p.Period).Select(g => new TradeVolume { LocalTime = GetTimeStamp(g.Key), Volume = g.Sum(v => v.Volume) });

                //Write the trade volumes to a file.
                string basePath = _configuration["TradesFolder"]!.ToString();
                FileHelpers.FileHelperEngine<TradeVolume> engine = new FileHelpers.FileHelperEngine<TradeVolume>();
                string filePath = Path.Combine(basePath, $"PowerPosition_{DateTime.Now:yyyyMMdd_hHHmm}.csv");

                engine.WriteFile(filePath, tradeVolumes);
            }
            else
            {
                _logger?.LogWarning($"No trades found for date {tradeDate:yyyy-MM-dd}.");
            }   
            //await Task.Delay(1000);
        }

        private string GetTimeStamp(int Period)
        {
            if (Period == 1)
            {
                return "00:00:00";
            }
            else if (Period == 2)
            {
                return "01:00:00";
            }
            else if (Period == 3)
            {
                return "02:00:00";
            }
            else if (Period == 4)
            {
                return "03:00:00";
            }
            else if (Period == 5)
            {
                return "04:00:00";
            }
            else if (Period == 6)
            {
                return "05:00:00";
            }
            else if (Period == 7)
            {
                return "06:00:00";
            }
            else if (Period == 8)
            {
                return "07:00:00";
            }
            else if (Period == 9)
            {
                return "08:00:00";
            }
            else if (Period == 10)
            {
                return "09:00:00";
            }
            else if (Period == 11)
            {
                return "10:00:00";
            }
            else if (Period == 12)
            {
                return "11:00:00";
            }
            else if (Period == 13)
            {
                return "12:00:00";
            }
            else if (Period == 14)
            {
                return "13:00:00";
            }
            else if (Period == 15)
            {
                return "14:00:00";
            }
            else if (Period == 16)
            {
                return "15:00:00";
            }
            else if (Period == 17)
            {
                return "16:00:00";
            }
            else if (Period == 18)
            {
                return "17:00:00";
            }
            else if (Period == 19)
            {
                return "18:00:00";
            }
            else if (Period == 20)
            {
                return "19:00:00";
            }
            else if (Period == 21)
            {
                return "20:00:00";
            }
            else if (Period == 22)
            {
                return "21:00:00";
            }
            else if (Period == 23)
            {
                return "22:00:00";
            }
            else if (Period == 24)
            {
                return "23:00:00";
            }

            return String.Empty;
        }
    }
}
