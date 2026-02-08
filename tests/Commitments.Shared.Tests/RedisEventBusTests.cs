using Commitments.Shared;
using FluentAssertions;
using Moq;
using StackExchange.Redis;
using Xunit;

namespace Commitments.Shared.Tests;

public class RedisEventBusTests
{
    [Fact]
    public async Task PublishAsync_SerializesAndPublishesToCorrectChannel()
    {
        var mockSubscriber = new Mock<ISubscriber>();
        var mockRedis = new Mock<IConnectionMultiplexer>();
        mockRedis.Setup(r => r.GetSubscriber(It.IsAny<object>())).Returns(mockSubscriber.Object);

        var bus = new RedisEventBus(mockRedis.Object);
        var @event = new ProfileCreatedEvent { ProfileId = Guid.NewGuid() };

        await bus.PublishAsync(@event);

        mockSubscriber.Verify(s => s.PublishAsync(
            It.Is<RedisChannel>(ch => ch.ToString() == nameof(ProfileCreatedEvent)),
            It.Is<RedisValue>(v => v.ToString().Contains(@event.ProfileId.ToString())),
            It.IsAny<CommandFlags>()), Times.Once);
    }

    [Fact]
    public async Task SubscribeAsync_SubscribesToCorrectChannel()
    {
        var mockSubscriber = new Mock<ISubscriber>();
        var mockRedis = new Mock<IConnectionMultiplexer>();
        mockRedis.Setup(r => r.GetSubscriber(It.IsAny<object>())).Returns(mockSubscriber.Object);

        var bus = new RedisEventBus(mockRedis.Object);

        await bus.SubscribeAsync<ProfileCreatedEvent>(e => Task.CompletedTask);

        mockSubscriber.Verify(s => s.SubscribeAsync(
            It.Is<RedisChannel>(ch => ch.ToString() == nameof(ProfileCreatedEvent)),
            It.IsAny<Action<RedisChannel, RedisValue>>(),
            It.IsAny<CommandFlags>()), Times.Once);
    }
}
