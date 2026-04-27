using Commitments.Api.Hubs;
using Commitments.Api.Realtime;
using Commitments.Shared.Realtime;
using FluentAssertions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Realtime;

public class SignalRRealtimePublisherTests
{
    private readonly Mock<IHubContext<CommitmentsHub>> _hub = new();
    private readonly Mock<IHubClients> _clients = new();
    private readonly Mock<IClientProxy> _clientProxy = new();
    private readonly Guid _profileId = Guid.Parse("11111111-2222-3333-4444-555555555555");

    public SignalRRealtimePublisherTests()
    {
        _clients.Setup(c => c.Group(It.IsAny<string>())).Returns(_clientProxy.Object);
        _hub.Setup(h => h.Clients).Returns(_clients.Object);
    }

    private SignalRRealtimePublisher CreatePublisher() =>
        new(_hub.Object, NullLogger<SignalRRealtimePublisher>.Instance);

    public sealed record TestPayload(string Text);

    private RealtimeMessage<TestPayload> CapturedEnvelope()
    {
        object?[]? captured = null;
        _clientProxy.Verify(p => p.SendCoreAsync(
            "message",
            Capture.In(new List<object?[]>(new[] { captured! })),
            It.IsAny<CancellationToken>()), Times.Once);

        var invocation = _clientProxy.Invocations.Single(i => i.Method.Name == "SendCoreAsync");
        var args = (object?[])invocation.Arguments[1]!;
        args.Should().HaveCount(1);
        return args[0].Should().BeOfType<RealtimeMessage<TestPayload>>().Which;
    }

    [Fact]
    public async Task PublishToProfileAsync_SendsToLowercaseProfileGroup()
    {
        var publisher = CreatePublisher();

        await publisher.PublishToProfileAsync(_profileId, "hubPing", new TestPayload("hello"));

        var expectedGroup = $"profile:{_profileId:D}".ToLowerInvariant();
        _clients.Verify(c => c.Group(expectedGroup), Times.Once);
    }

    [Fact]
    public async Task PublishToProfileAsync_SendsOnMessageMethod()
    {
        var publisher = CreatePublisher();

        await publisher.PublishToProfileAsync(_profileId, "hubPing", new TestPayload("hello"));

        _clientProxy.Verify(p => p.SendCoreAsync(
            "message",
            It.IsAny<object?[]>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task PublishToProfileAsync_BuildsEnvelopeWithSchemaVersion1()
    {
        var publisher = CreatePublisher();

        await publisher.PublishToProfileAsync(_profileId, "hubPing", new TestPayload("hello"));

        var envelope = CapturedEnvelope();
        envelope.SchemaVersion.Should().Be(1);
    }

    [Fact]
    public async Task PublishToProfileAsync_BuildsEnvelopeWithEventNameAndProfileIdAndPayload()
    {
        var publisher = CreatePublisher();

        await publisher.PublishToProfileAsync(_profileId, "hubPing", new TestPayload("hello"));

        var envelope = CapturedEnvelope();
        envelope.Event.Should().Be("hubPing");
        envelope.ProfileId.Should().Be(_profileId);
        envelope.Payload.Should().Be(new TestPayload("hello"));
    }

    [Fact]
    public async Task PublishToProfileAsync_GeneratesNewMessageIdEachCall()
    {
        var publisher = CreatePublisher();

        await publisher.PublishToProfileAsync(_profileId, "hubPing", new TestPayload("a"));
        await publisher.PublishToProfileAsync(_profileId, "hubPing", new TestPayload("b"));

        var invocations = _clientProxy.Invocations.Where(i => i.Method.Name == "SendCoreAsync").ToList();
        invocations.Should().HaveCount(2);
        var env1 = (RealtimeMessage<TestPayload>)((object?[])invocations[0].Arguments[1]!)[0]!;
        var env2 = (RealtimeMessage<TestPayload>)((object?[])invocations[1].Arguments[1]!)[0]!;
        env1.MessageId.Should().NotBeEmpty();
        env2.MessageId.Should().NotBeEmpty();
        env1.MessageId.Should().NotBe(env2.MessageId);
    }

    [Fact]
    public async Task PublishToProfileAsync_PassesCorrelationIdThrough()
    {
        var publisher = CreatePublisher();
        var correlationId = Guid.NewGuid();

        await publisher.PublishToProfileAsync(_profileId, "hubPing", new TestPayload("hello"), correlationId);

        var envelope = CapturedEnvelope();
        envelope.CorrelationId.Should().Be(correlationId);
    }
}
