using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackgroundQueue.Controllers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BackgroundQueue {

    public interface IBackgroundPublisher {
        void Publish<TNotification>(TNotification notification) where TNotification : INotification;
    }

    public class BackgroundPublisher : IBackgroundPublisher {
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public BackgroundPublisher(
            IBackgroundTaskQueue backgroundTaskQueue,
            IServiceScopeFactory serviceScopeFactory
            ) {
            _backgroundTaskQueue = backgroundTaskQueue;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Publish<TNotification>(TNotification notification) where TNotification : INotification
            => _backgroundTaskQueue.QueueBackgroundWorkItem(async cancellationToken => {

                using (var scope = _serviceScopeFactory.CreateScope()) {

                    var mediator = scope.ServiceProvider.GetService<IMediator>();

                    await mediator.Publish(
                        notification,
                        cancellationToken
                        );

                }

            });


    }

}
