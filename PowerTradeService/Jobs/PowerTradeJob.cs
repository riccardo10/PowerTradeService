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
                var tradeVolumes = tradePeriods.GroupBy(p => p.Period).Select(g => new TradeVolume { LocalTime = GetTimeStamp(g.Key), Volume = g.Sum(v => v.Volume) });

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
            }
            else
            {
                _logger?.LogWarning($"No trades found for date {tradeDate:yyyy-MM-dd}.");
            }   
        }

        private string GetTimeStamp(int Period)
        {
            if (Period == 1)
            {
                return "23:00";
            }
            else if (Period == 2)
            {
                return "00:00";
            }
            else if (Period == 3)
            {
                return "01:00";
            }
            else if (Period == 4)
            {
                return "02:00";
            }
            else if (Period == 5)
            {
                return "03:00";
            }
            else if (Period == 6)
            {
                return "04:00";
            }
            else if (Period == 7)
            {
                return "05:00";
            }
            else if (Period == 8)
            {
                return "06:00";
            }
            else if (Period == 9)
            {
                return "07:00";
            }
            else if (Period == 10)
            {
                return "08:00";
            }
            else if (Period == 11)
            {
                return "09:00";
            }
            else if (Period == 12)
            {
                return "10:00";
            }
            else if (Period == 13)
            {
                return "11:00";
            }
            else if (Period == 14)
            {
                return "12:00";
            }
            else if (Period == 15)
            {
                return "13:00";
            }
            else if (Period == 16)
            {
                return "14:00";
            }
            else if (Period == 17)
            {
                return "15:00";
            }
            else if (Period == 18)
            {
                return "16:00";
            }
            else if (Period == 19)
            {
                return "17:00";
            }
            else if (Period == 20)
            {
                return "18:00";
            }
            else if (Period == 21)
            {
                return "19:00";
            }
            else if (Period == 22)
            {
                return "20:00";
            }
            else if (Period == 23)
            {
                return "21:00";
            }
            else if (Period == 24)
            {
                return "22:00";
            }

            return String.Empty;
        }
    }
}
