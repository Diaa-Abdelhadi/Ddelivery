using Ddelivery.BLL.Service;

namespace Ddelivery.PL.BackgroundJobs
{
    public class OrderCancellationJob : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _checkInterval;
        private readonly int _abandonThresholdMinutes;
        private readonly ILogger<OrderCancellationJob> _logger;

        public OrderCancellationJob(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<OrderCancellationJob> logger)
        {
            _serviceProvider = serviceProvider;
            _checkInterval = TimeSpan.FromSeconds(configuration.GetValue<int>("OrderCancellation:CheckIntervalSeconds", 60));
            _abandonThresholdMinutes = configuration.GetValue<int>("OrderCancellation:AbandonThresholdMinutes", 15);
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
                    var cancelledCount = await orderService.CancelStaleOrdersAsync(_abandonThresholdMinutes);
                    if (cancelledCount > 0)
                    {
                        _logger.LogInformation("Auto-cancelled {Count} abandoned order(s).", cancelledCount);
                    }
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }
        }
    }
}
