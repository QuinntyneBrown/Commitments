using Commitments.Domain.ActivityAggregate;
using Commitments.Domain.BehaviourAggregate;
using Commitments.Domain.BehaviourTypeAggregate;
using Commitments.Features.WeeklyFocus;
using Commitments.Testing.Common;
using FluentAssertions;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Queries;

public class GetWeeklyFocusHandlerTests
{
    private static Behaviour Behaviour(string name)
    {
        var behaviourType = new BehaviourType { BehaviourTypeId = Guid.NewGuid(), Name = "Type" };
        return new Behaviour
        {
            BehaviourId = Guid.NewGuid(),
            Name = name,
            BehaviourType = behaviourType,
            BehaviourTypeId = behaviourType.BehaviourTypeId
        };
    }

    [Fact]
    public async Task Handle_NoActivities_ReturnsEmptyFocusAreas()
    {
        var profileId = Guid.NewGuid();
        var asOf = new DateTimeOffset(2026, 4, 22, 12, 0, 0, TimeSpan.Zero);
        var mockContext = MockCommitmentsDbContextFactory.Create();

        var handler = new GetWeeklyFocusHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetWeeklyFocusRequest { ProfileId = profileId, AsOf = asOf },
            CancellationToken.None);

        response.FocusAreas.Should().BeEmpty();
        response.WeekStart.Should().Be("2026-04-20");
        response.WeekEnd.Should().Be("2026-04-26");
    }

    [Fact]
    public async Task Handle_ReturnsTop3BehavioursByActivityCountInWeek()
    {
        var profileId = Guid.NewGuid();
        var asOf = new DateTimeOffset(2026, 4, 22, 12, 0, 0, TimeSpan.Zero);
        var bMove = Behaviour("Move");
        var bRead = Behaviour("Read");
        var bReflect = Behaviour("Reflect");
        var bExtra = Behaviour("Extra");

        DateTime InWeek(int hours) => new DateTime(2026, 4, 21, 0, 0, 0, DateTimeKind.Utc).AddHours(hours);

        var activities = new List<Activity>
        {
            new() { ActivityId = Guid.NewGuid(), ProfileId = profileId, BehaviourId = bMove.BehaviourId, Behaviour = bMove, PerformedOn = InWeek(1) },
            new() { ActivityId = Guid.NewGuid(), ProfileId = profileId, BehaviourId = bMove.BehaviourId, Behaviour = bMove, PerformedOn = InWeek(2) },
            new() { ActivityId = Guid.NewGuid(), ProfileId = profileId, BehaviourId = bMove.BehaviourId, Behaviour = bMove, PerformedOn = InWeek(3) },
            new() { ActivityId = Guid.NewGuid(), ProfileId = profileId, BehaviourId = bMove.BehaviourId, Behaviour = bMove, PerformedOn = InWeek(4) },
            new() { ActivityId = Guid.NewGuid(), ProfileId = profileId, BehaviourId = bMove.BehaviourId, Behaviour = bMove, PerformedOn = InWeek(5) },
            new() { ActivityId = Guid.NewGuid(), ProfileId = profileId, BehaviourId = bRead.BehaviourId, Behaviour = bRead, PerformedOn = InWeek(6) },
            new() { ActivityId = Guid.NewGuid(), ProfileId = profileId, BehaviourId = bRead.BehaviourId, Behaviour = bRead, PerformedOn = InWeek(7) },
            new() { ActivityId = Guid.NewGuid(), ProfileId = profileId, BehaviourId = bRead.BehaviourId, Behaviour = bRead, PerformedOn = InWeek(8) },
            new() { ActivityId = Guid.NewGuid(), ProfileId = profileId, BehaviourId = bReflect.BehaviourId, Behaviour = bReflect, PerformedOn = InWeek(9) },
            new() { ActivityId = Guid.NewGuid(), ProfileId = profileId, BehaviourId = bReflect.BehaviourId, Behaviour = bReflect, PerformedOn = InWeek(10) },
            new() { ActivityId = Guid.NewGuid(), ProfileId = profileId, BehaviourId = bExtra.BehaviourId, Behaviour = bExtra, PerformedOn = InWeek(11) }
        };

        var mockContext = MockCommitmentsDbContextFactory.Create();
        mockContext.Setup(c => c.Activities)
            .Returns(MockDbSetFactory.CreateMockDbSet(activities).Object);

        var handler = new GetWeeklyFocusHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetWeeklyFocusRequest { ProfileId = profileId, AsOf = asOf },
            CancellationToken.None);

        response.FocusAreas.Should().HaveCount(3);
        response.FocusAreas[0].Name.Should().Be("Move");
        response.FocusAreas[0].Rank.Should().Be(1);
        response.FocusAreas[0].SupportingMetric.Should().Be("5 sessions logged");
        response.FocusAreas[1].Name.Should().Be("Read");
        response.FocusAreas[1].Rank.Should().Be(2);
        response.FocusAreas[2].Name.Should().Be("Reflect");
        response.FocusAreas[2].Rank.Should().Be(3);
    }

    [Fact]
    public async Task Handle_ActivitiesOutsideWeek_NotCounted()
    {
        var profileId = Guid.NewGuid();
        var asOf = new DateTimeOffset(2026, 4, 22, 12, 0, 0, TimeSpan.Zero);
        var b = Behaviour("OutOfWindow");

        var activities = new List<Activity>
        {
            new() { ActivityId = Guid.NewGuid(), ProfileId = profileId, BehaviourId = b.BehaviourId, Behaviour = b, PerformedOn = new DateTime(2026, 4, 19, 12, 0, 0, DateTimeKind.Utc) },
            new() { ActivityId = Guid.NewGuid(), ProfileId = profileId, BehaviourId = b.BehaviourId, Behaviour = b, PerformedOn = new DateTime(2026, 4, 27, 12, 0, 0, DateTimeKind.Utc) }
        };

        var mockContext = MockCommitmentsDbContextFactory.Create();
        mockContext.Setup(c => c.Activities)
            .Returns(MockDbSetFactory.CreateMockDbSet(activities).Object);

        var handler = new GetWeeklyFocusHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetWeeklyFocusRequest { ProfileId = profileId, AsOf = asOf },
            CancellationToken.None);

        response.FocusAreas.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ActivitiesAfterAsOfInSameWeek_NotCounted()
    {
        var profileId = Guid.NewGuid();
        var asOf = new DateTimeOffset(2026, 4, 22, 12, 0, 0, TimeSpan.Zero);
        var b = Behaviour("Move");

        var activities = new List<Activity>
        {
            new() { ActivityId = Guid.NewGuid(), ProfileId = profileId, BehaviourId = b.BehaviourId, Behaviour = b, PerformedOn = asOf.UtcDateTime.AddHours(1) }
        };

        var mockContext = MockCommitmentsDbContextFactory.Create();
        mockContext.Setup(c => c.Activities)
            .Returns(MockDbSetFactory.CreateMockDbSet(activities).Object);

        var handler = new GetWeeklyFocusHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetWeeklyFocusRequest { ProfileId = profileId, AsOf = asOf },
            CancellationToken.None);

        response.FocusAreas.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_NullAsOf_ModeIsLiveAndAsOfNearUtcNow()
    {
        var profileId = Guid.NewGuid();
        var mockContext = MockCommitmentsDbContextFactory.Create();

        var handler = new GetWeeklyFocusHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetWeeklyFocusRequest { ProfileId = profileId, AsOf = null },
            CancellationToken.None);

        response.Mode.Should().Be("live");
        response.AsOf.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task Handle_AsOfProvided_ModeIsReview()
    {
        var profileId = Guid.NewGuid();
        var asOf = new DateTimeOffset(2026, 4, 22, 12, 0, 0, TimeSpan.Zero);
        var mockContext = MockCommitmentsDbContextFactory.Create();

        var handler = new GetWeeklyFocusHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetWeeklyFocusRequest { ProfileId = profileId, AsOf = asOf },
            CancellationToken.None);

        response.Mode.Should().Be("review");
        response.AsOf.Should().Be(asOf);
    }

    [Fact]
    public async Task Handle_FutureAsOf_ClampedToUtcNow()
    {
        var profileId = Guid.NewGuid();
        var future = DateTimeOffset.UtcNow.AddYears(10);
        var mockContext = MockCommitmentsDbContextFactory.Create();

        var handler = new GetWeeklyFocusHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetWeeklyFocusRequest { ProfileId = profileId, AsOf = future },
            CancellationToken.None);

        response.AsOf.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));
    }
}
