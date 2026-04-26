using Commitments.Shared;

namespace Commitments.Testing.Common;

public class NullEventBus : IEventBus
{
    public Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : IIntegrationEvent
        => Task.CompletedTask;

    public Task SubscribeAsync<T>(Func<T, Task> handler, CancellationToken cancellationToken = default) where T : IIntegrationEvent
        => Task.CompletedTask;
}
