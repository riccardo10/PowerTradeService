using Quartz;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;

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
                var tradeVolumes = tradePeriods.GroupBy(p => p.Period).Select(g => new TradeVolume { LocalTime = GetLocalTime(g.Key), Volume = g.Sum(v => v.Volume) });

                //Write the trade volumes to a .csv file.
                string basePath = _configuration["TradesFolder"]!.ToString();

                // Ensure the directory exists.
                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }

                string filePath = Path.Combine(basePath, $"PowerPosition_{DateTime.Now:yyyyMMdd_HHmm}.csv");

                using StreamWriter streamWriter = new StreamWriter(filePath);
                using CsvWriter csvWriter = new CsvWriter(streamWriter, System.Globalization.CultureInfo.InvariantCulture);
                csvWriter.WriteRecords(tradeVolumes);
                csvWriter.Flush();

                _logger?.LogInformation($"Trade volumes for date {tradeDate:yyyy-MM-dd} have been written to {filePath}.");
            }
            else
            {
                _logger?.LogWarning($"No trades found for date {tradeDate:yyyy-MM-dd}.");
            }   
        }

        private string GetLocalTime(int Period)
        {
            string localTime = Period switch
            {
                1 => "23:00",
                2 => "00:00",
                3 => "01:00",
                4 => "02:00",
                5 => "03:00",
                6 => "04:00",
                7 => "05:00",
                8 => "06:00",
                9 => "07:00",
               10 => "08:00",
               11 => "09:00",
               12 => "10:00",
               13 => "11:00",
               14 => "12:00",
               15 => "13:00",
               16 => "14:00",
               17 => "15:00",
               18 => "16:00",
               19 => "17:00",
               20 => "18:00",
               21 => "19:00",
               22 => "20:00",
               23 => "21:00",
               24 => "22:00",
                _ => "N/A"
            };

            return localTime;
        }
    }
}
