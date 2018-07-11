using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackgroundPublishWithMediator.Controllers {

    public class MyNotification : INotification {
        public string Value { get; set; }
    }

    public class MyNotificationHandler1 : INotificationHandler<MyNotification> {
        private readonly ILogger<MyNotificationHandler1> _logger;

        public MyNotificationHandler1(
            ILogger<MyNotificationHandler1> logger
            ) {
            _logger = logger;
        }

        public async Task Handle(MyNotification notification, CancellationToken cancellationToken) {

            // just log for now
            _logger.LogInformation("Handling notification for {name}", nameof(MyNotificationHandler1));

            await Task.Delay(5000);

            _logger.LogInformation("Notification for {name} completed", nameof(MyNotificationHandler1));
            
        }
    }

    public class MyNotificationHandler2 : INotificationHandler<MyNotification> {
        private readonly ILogger<MyNotificationHandler2> _logger;

        public MyNotificationHandler2(
            ILogger<MyNotificationHandler2> logger
            ) {
            _logger = logger;
        }

        public Task Handle(MyNotification notification, CancellationToken cancellationToken) {

            // just log for now
            _logger.LogInformation("Handling notification for {name}", nameof(MyNotificationHandler2));

            return Task.CompletedTask;

        }
    }

}