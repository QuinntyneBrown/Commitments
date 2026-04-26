using Commitments.Features.GoalProgress;
using FluentAssertions;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Queries;

public class CacheControlPolicyTests
{
    [Fact]
    public void Always_NoStore_ForNullAsOf()
    {
        var now = DateTimeOffset.UtcNow;

        var header = CacheControlPolicy.For(asOf: null, utcNow: now);

        header.Should().Be("no-store");
    }

    [Fact]
    public void NoStore_WhenAsOfWithinFreshnessWindow()
    {
        var now = DateTimeOffset.UtcNow;
        var asOf = now.AddSeconds(-10);

        var header = CacheControlPolicy.For(asOf: asOf, utcNow: now);

        header.Should().Be("no-store");
    }

    [Fact]
    public void PublicMaxAge_WhenAsOfFullyHistorical()
    {
        var now = DateTimeOffset.UtcNow;
        var asOf = now.AddMinutes(-5);

        var header = CacheControlPolicy.For(asOf: asOf, utcNow: now);

        header.Should().Be("public, max-age=300");
    }

    [Fact]
    public void PublicMaxAge_AtFreshnessBoundary()
    {
        var now = DateTimeOffset.UtcNow;
        var asOf = now.AddMinutes(-1).AddSeconds(-1);

        var header = CacheControlPolicy.For(asOf: asOf, utcNow: now);

        header.Should().Be("public, max-age=300");
    }

    [Fact]
    public void NoStore_WhenAsOfInFuture()
    {
        var now = DateTimeOffset.UtcNow;
        var asOf = now.AddMinutes(5);

        var header = CacheControlPolicy.For(asOf: asOf, utcNow: now);

        header.Should().Be("no-store");
    }
}
