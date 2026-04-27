using System.Text.Json;
using Commitments.Shared.Realtime;
using FluentAssertions;
using Xunit;

namespace Commitments.Shared.Tests.Realtime;

public class RealtimeMessageTests
{
    public sealed record SamplePayload(string Text, int N);

    [Fact]
    public void Envelope_SerializesAsCamelCase_WithCamelCasePolicy()
    {
        var envelope = new RealtimeMessage<SamplePayload>(
            SchemaVersion: 1,
            MessageId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Event: "hubPing",
            ProfileId: Guid.Parse("22222222-2222-2222-2222-222222222222"),
            OccurredAt: DateTimeOffset.Parse("2026-04-26T12:00:00Z"),
            CorrelationId: null,
            Payload: new SamplePayload("hello", 7));

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(envelope, options);

        json.Should().Contain("\"schemaVersion\":1");
        json.Should().Contain("\"messageId\":");
        json.Should().Contain("\"event\":\"hubPing\"");
        json.Should().Contain("\"profileId\":");
        json.Should().Contain("\"occurredAt\":");
        json.Should().Contain("\"correlationId\":null");
        json.Should().Contain("\"payload\":");
        json.Should().Contain("\"text\":\"hello\"");
        json.Should().Contain("\"n\":7");
    }
}
