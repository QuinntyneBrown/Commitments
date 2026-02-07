// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Text.Json;
using StackExchange.Redis;

namespace Commitments.Shared;

public class RedisEventBus : IEventBus
{
    private readonly IConnectionMultiplexer _redis;

    public RedisEventBus(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : IIntegrationEvent
    {
        var subscriber = _redis.GetSubscriber();
        var channel = typeof(T).Name;
        var message = JsonSerializer.Serialize(@event);
        await subscriber.PublishAsync(RedisChannel.Literal(channel), message);
    }

    public async Task SubscribeAsync<T>(Func<T, Task> handler, CancellationToken cancellationToken = default) where T : IIntegrationEvent
    {
        var subscriber = _redis.GetSubscriber();
        var channel = typeof(T).Name;
        await subscriber.SubscribeAsync(RedisChannel.Literal(channel), async (ch, message) =>
        {
            var @event = JsonSerializer.Deserialize<T>(message!);
            if (@event != null)
            {
                await handler(@event);
            }
        });
    }
}
