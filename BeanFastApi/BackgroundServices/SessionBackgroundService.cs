using Services.Implements;
using Services.Interfaces;
using Utilities.Constants;
using Utilities.Utils;

namespace BeanFastApi.BackgroundJobs
{
    public class SessionBackgroundService : BackgroundService
    {
        public IServiceProvider Services { get; }
        public SessionBackgroundService(IServiceProvider services)
        {
            Services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //await _sessionService.UpdateOrdersStatusAutoAsync();
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = Services.CreateScope())
                {

                    var sessionService =
                        scope.ServiceProvider
                            .GetRequiredService<ISessionService>();
                    await sessionService.UpdateOrdersStatusAutoAsync();
                    Console.WriteLine("Background job is running with delayed: " + BackgroundServiceConstrant.DelayedInMinutes + " minutes");
                    Console.WriteLine(TimeUtil.GetCurrentVietNamTime());
                    await Task.Delay(TimeSpan.FromMinutes(BackgroundServiceConstrant.DelayedInMinutes), stoppingToken);
                }
            }
            
        }
    }
}
