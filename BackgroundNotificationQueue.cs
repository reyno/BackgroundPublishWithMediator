using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

namespace BackgroundPublishWithMediator {


    public interface IBackgroundNotificationQueue {
        void Enqueue<TNotification>(TNotification notification) where TNotification : INotification;
        Task<INotification> DequeueAsync(CancellationToken cancellationToken);
    }

    public class BackgroundNotificationQueue : IBackgroundNotificationQueue {

        private ConcurrentQueue<INotification> _notifications =
            new ConcurrentQueue<INotification>();
        private SemaphoreSlim _signal = new SemaphoreSlim(0);

        public async Task<INotification> DequeueAsync(CancellationToken cancellationToken) {
            await _signal.WaitAsync(cancellationToken);
            _notifications.TryDequeue(out var notification);
            return notification;
        }

        public void Enqueue<TNotification>(TNotification notification) where TNotification : INotification {
            _notifications.Enqueue(notification);
            _signal.Release();
        }

    }
}
