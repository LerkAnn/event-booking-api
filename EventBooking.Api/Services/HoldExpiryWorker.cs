namespace EventBooking.Api.Services;

public class HoldExpiryWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;

    public HoldExpiryWorker(IServiceScopeFactory scopeFactory, IConfiguration configuration)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var holdMinutes = _configuration.GetValue<int>("HoldExpiry:HoldMinutes");
        var holdSeconds = _configuration.GetValue<int>("HoldExpiry:IntervalSeconds");
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var bookingService = scope.ServiceProvider.GetRequiredService<IBookingService>();
                var count = await bookingService.ExpireHoldsAsync(TimeSpan.FromMinutes(holdMinutes));
                Console.WriteLine($"Expiry job: {count} hold(s) expired");
            }

            await Task.Delay(TimeSpan.FromSeconds(holdSeconds), stoppingToken);
        }
    }
}