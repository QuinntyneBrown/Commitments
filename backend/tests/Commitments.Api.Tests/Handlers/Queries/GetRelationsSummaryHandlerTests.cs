using Commitments.Domain.BehaviourAggregate;
using Commitments.Domain.BehaviourTypeAggregate;
using Commitments.Domain.CommitmentAggregate;
using Commitments.Features.Relations;
using Commitments.Testing.Common;
using FluentAssertions;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Queries;

public class GetRelationsSummaryHandlerTests
{
    private static (Commitment commitment, Behaviour behaviour, BehaviourType behaviourType) BuildCommitment(
        Guid profileId,
        BehaviourType behaviourType,
        DateTime createdOn)
    {
        var behaviour = new Behaviour
        {
            BehaviourId = Guid.NewGuid(),
            Name = "Behaviour",
            BehaviourType = behaviourType,
            BehaviourTypeId = behaviourType.BehaviourTypeId
        };
        var commitment = new Commitment
        {
            CommitmentId = Guid.NewGuid(),
            ProfileId = profileId,
            BehaviourId = behaviour.BehaviourId,
            Behaviour = behaviour,
            CreatedOn = createdOn
        };
        return (commitment, behaviour, behaviourType);
    }

    [Fact]
    public async Task Handle_NoCommitments_ReturnsEmptyRelationsAndZeroTotal()
    {
        var profileId = Guid.NewGuid();
        var asOf = new DateTimeOffset(2026, 4, 27, 12, 0, 0, TimeSpan.Zero);
        var mockContext = MockCommitmentsDbContextFactory.Create();

        var handler = new GetRelationsSummaryHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetRelationsSummaryRequest { ProfileId = profileId, AsOf = asOf, Top = 3 },
            CancellationToken.None);

        response.Relations.Should().BeEmpty();
        response.TotalCommitments.Should().Be(0);
    }

    [Fact]
    public async Task Handle_ThreeCategories_ReturnsCountsAndPercentagesSummingTo100()
    {
        var profileId = Guid.NewGuid();
        var asOf = new DateTimeOffset(2026, 4, 27, 12, 0, 0, TimeSpan.Zero);
        var health = new BehaviourType { BehaviourTypeId = Guid.NewGuid(), Name = "Health" };
        var work = new BehaviourType { BehaviourTypeId = Guid.NewGuid(), Name = "Work" };
        var personal = new BehaviourType { BehaviourTypeId = Guid.NewGuid(), Name = "Personal" };

        var commitments = new List<Commitment>();
        for (var i = 0; i < 5; i++) commitments.Add(BuildCommitment(profileId, health, asOf.UtcDateTime.AddDays(-10)).commitment);
        for (var i = 0; i < 4; i++) commitments.Add(BuildCommitment(profileId, work, asOf.UtcDateTime.AddDays(-10)).commitment);
        for (var i = 0; i < 3; i++) commitments.Add(BuildCommitment(profileId, personal, asOf.UtcDateTime.AddDays(-10)).commitment);

        var mockContext = MockCommitmentsDbContextFactory.Create();
        mockContext.Setup(c => c.Commitments)
            .Returns(MockDbSetFactory.CreateMockDbSet(commitments).Object);

        var handler = new GetRelationsSummaryHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetRelationsSummaryRequest { ProfileId = profileId, AsOf = asOf, Top = 3 },
            CancellationToken.None);

        response.TotalCommitments.Should().Be(12);
        response.Relations.Should().HaveCount(3);
        response.Relations[0].Name.Should().Be("Health");
        response.Relations[0].Count.Should().Be(5);
        response.Relations[1].Name.Should().Be("Work");
        response.Relations[1].Count.Should().Be(4);
        response.Relations[2].Name.Should().Be("Personal");
        response.Relations[2].Count.Should().Be(3);
        response.Relations.Sum(r => r.Percentage).Should().Be(100);
    }

    [Fact]
    public async Task Handle_CommitmentCreatedAfterAsOf_NotCounted()
    {
        var profileId = Guid.NewGuid();
        var asOf = new DateTimeOffset(2026, 4, 27, 12, 0, 0, TimeSpan.Zero);
        var health = new BehaviourType { BehaviourTypeId = Guid.NewGuid(), Name = "Health" };

        var (oldCommitment, _, _) = BuildCommitment(profileId, health, asOf.UtcDateTime.AddDays(-3));
        var (futureCommitment, _, _) = BuildCommitment(profileId, health, asOf.UtcDateTime.AddDays(3));

        var mockContext = MockCommitmentsDbContextFactory.Create();
        mockContext.Setup(c => c.Commitments)
            .Returns(MockDbSetFactory.CreateMockDbSet(new List<Commitment> { oldCommitment, futureCommitment }).Object);

        var handler = new GetRelationsSummaryHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetRelationsSummaryRequest { ProfileId = profileId, AsOf = asOf, Top = 3 },
            CancellationToken.None);

        response.TotalCommitments.Should().Be(1);
    }

    [Fact]
    public async Task Handle_OtherProfileCommitments_NotCounted()
    {
        var profileId = Guid.NewGuid();
        var otherProfileId = Guid.NewGuid();
        var asOf = new DateTimeOffset(2026, 4, 27, 12, 0, 0, TimeSpan.Zero);
        var health = new BehaviourType { BehaviourTypeId = Guid.NewGuid(), Name = "Health" };

        var (mine, _, _) = BuildCommitment(profileId, health, asOf.UtcDateTime.AddDays(-3));
        var (theirs, _, _) = BuildCommitment(otherProfileId, health, asOf.UtcDateTime.AddDays(-3));

        var mockContext = MockCommitmentsDbContextFactory.Create();
        mockContext.Setup(c => c.Commitments)
            .Returns(MockDbSetFactory.CreateMockDbSet(new List<Commitment> { mine, theirs }).Object);

        var handler = new GetRelationsSummaryHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetRelationsSummaryRequest { ProfileId = profileId, AsOf = asOf, Top = 3 },
            CancellationToken.None);

        response.TotalCommitments.Should().Be(1);
    }

    [Fact]
    public async Task Handle_TopParameter_LimitsRows()
    {
        var profileId = Guid.NewGuid();
        var asOf = new DateTimeOffset(2026, 4, 27, 12, 0, 0, TimeSpan.Zero);
        var commitments = new List<Commitment>();
        for (var i = 0; i < 5; i++)
        {
            var bt = new BehaviourType { BehaviourTypeId = Guid.NewGuid(), Name = $"Cat{i}" };
            for (var j = 0; j < (5 - i); j++)
            {
                commitments.Add(BuildCommitment(profileId, bt, asOf.UtcDateTime.AddDays(-3)).commitment);
            }
        }

        var mockContext = MockCommitmentsDbContextFactory.Create();
        mockContext.Setup(c => c.Commitments)
            .Returns(MockDbSetFactory.CreateMockDbSet(commitments).Object);

        var handler = new GetRelationsSummaryHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetRelationsSummaryRequest { ProfileId = profileId, AsOf = asOf, Top = 2 },
            CancellationToken.None);

        response.Relations.Should().HaveCount(2);
        response.Relations[0].Name.Should().Be("Cat0");
        response.Relations[1].Name.Should().Be("Cat1");
    }

    [Fact]
    public async Task Handle_TopOver10_CappedAt10()
    {
        var profileId = Guid.NewGuid();
        var asOf = new DateTimeOffset(2026, 4, 27, 12, 0, 0, TimeSpan.Zero);
        var commitments = new List<Commitment>();
        for (var i = 0; i < 12; i++)
        {
            var bt = new BehaviourType { BehaviourTypeId = Guid.NewGuid(), Name = $"Cat{i}" };
            commitments.Add(BuildCommitment(profileId, bt, asOf.UtcDateTime.AddDays(-3)).commitment);
        }

        var mockContext = MockCommitmentsDbContextFactory.Create();
        mockContext.Setup(c => c.Commitments)
            .Returns(MockDbSetFactory.CreateMockDbSet(commitments).Object);

        var handler = new GetRelationsSummaryHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetRelationsSummaryRequest { ProfileId = profileId, AsOf = asOf, Top = 99 },
            CancellationToken.None);

        response.Relations.Should().HaveCount(10);
    }

    [Fact]
    public async Task Handle_NullAsOf_ModeIsLive()
    {
        var profileId = Guid.NewGuid();
        var mockContext = MockCommitmentsDbContextFactory.Create();

        var handler = new GetRelationsSummaryHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetRelationsSummaryRequest { ProfileId = profileId, AsOf = null, Top = 3 },
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

        var handler = new GetRelationsSummaryHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetRelationsSummaryRequest { ProfileId = profileId, AsOf = asOf, Top = 3 },
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

        var handler = new GetRelationsSummaryHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetRelationsSummaryRequest { ProfileId = profileId, AsOf = future, Top = 3 },
            CancellationToken.None);

        response.AsOf.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));
    }
}
