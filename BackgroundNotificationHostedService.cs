using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace BackgroundPublishWithMediator {
    public class BackgroundNotificationHostedService : IHostedService {
        private CancellationTokenSource _shutdown = new CancellationTokenSource();
        private Task _backgroundTask;
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public BackgroundNotificationHostedService(
            IBackgroundNotificationQueue taskQueue,
            IServiceScopeFactory serviceScopeFactory,
            ILoggerFactory loggerFactory
            ) {
            TaskQueue = taskQueue;
            _logger = loggerFactory.CreateLogger<BackgroundNotificationHostedService>();
            _mediator = serviceScopeFactory.CreateScope().ServiceProvider.GetService<IMediator>();
        }

        public IBackgroundNotificationQueue TaskQueue { get; }

        public Task StartAsync(CancellationToken cancellationToken) {
            _logger.LogInformation("Queued Hosted Service is starting.");

            _backgroundTask = Task.Run(BackgroundProceessing);

            return Task.CompletedTask;
        }

        private async Task BackgroundProceessing() {
            while (!_shutdown.IsCancellationRequested) {

                var workItem = await TaskQueue.DequeueAsync(_shutdown.Token);

                try {
                    await _mediator.Publish(workItem, _shutdown.Token);
                } catch (Exception ex) {
                    _logger.LogError(ex, $"Error occurred executing {nameof(workItem)}.");
                }

            }
        }

        public Task StopAsync(CancellationToken cancellationToken) {
            _logger.LogInformation("Queued Hosted Service is stopping.");

            _shutdown.Cancel();

            return Task.WhenAny(_backgroundTask, Task.Delay(Timeout.Infinite, cancellationToken));

        }
    }
}