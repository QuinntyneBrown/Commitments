// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Concurrent;

namespace Commitments.Shared;

public class InMemoryEventBus : IEventBus
{
    private readonly ConcurrentDictionary<Type, List<Func<object, Task>>> _handlers = new();

    public async Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : IIntegrationEvent
    {
        if (!_handlers.TryGetValue(typeof(T), out var handlers))
        {
            return;
        }

        foreach (var handler in handlers.ToArray())
        {
            await handler(@event!);
        }
    }

    public Task SubscribeAsync<T>(Func<T, Task> handler, CancellationToken cancellationToken = default) where T : IIntegrationEvent
    {
        var list = _handlers.GetOrAdd(typeof(T), _ => new List<Func<object, Task>>());
        lock (list)
        {
            list.Add(o => handler((T)o));
        }
        return Task.CompletedTask;
    }
}
