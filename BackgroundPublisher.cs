using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackgroundPublishWithMediator;
using MediatR;

namespace BackgroundPublishWithMediator
{
    public interface IBackgroundPublisher {
        void Publish<TNotification>(TNotification notification) where TNotification : INotification;
    }

    public class BackgroundPublisher : IBackgroundPublisher {
        private readonly IBackgroundNotificationQueue _backgroundNotificationQueue;

        public BackgroundPublisher(
            IBackgroundNotificationQueue backgroundNotificationQueue
            ) {
            _backgroundNotificationQueue = backgroundNotificationQueue;
        }

        public void Publish<TNotification>(TNotification notification) where TNotification : INotification {
            _backgroundNotificationQueue.Enqueue(notification);
        }
    }
}
