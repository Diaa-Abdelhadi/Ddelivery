using Ddelivery.BLL.Service;

namespace Ddelivery.PL.BackgroundJobs
{
    public class DailyEarningsJob : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _checkInterval;
        private readonly ILogger<DailyEarningsJob> _logger;

        public DailyEarningsJob(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<DailyEarningsJob> logger)
        {
            _serviceProvider = serviceProvider;
            _checkInterval = TimeSpan.FromSeconds(configuration.GetValue<int>("Earnings:CheckIntervalSeconds", 3600));
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var earningsService = scope.ServiceProvider.GetRequiredService<IEarningsService>();
                    var yesterday = DateTime.UtcNow.Date.AddDays(-1);
                    var recordCount = await earningsService.CalculateDailyEarningsAsync(yesterday);
                    if (recordCount > 0)
                    {
                        _logger.LogInformation("Calculated daily earnings for {Date}: {Count} record(s).", yesterday.ToShortDateString(), recordCount);
                    }
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }
        }
    }
}
