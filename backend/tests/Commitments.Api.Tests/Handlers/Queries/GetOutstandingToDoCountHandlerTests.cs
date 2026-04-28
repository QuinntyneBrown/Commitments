using Commitments.Domain.ToDoAggregate;
using Commitments.Features.ToDo;
using Commitments.Testing.Common;
using FluentAssertions;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Queries;

public class GetOutstandingToDoCountHandlerTests
{
    private static ToDo MakeToDo(
        Guid profileId,
        DateTime createdOn,
        DateTime? completedOn = null,
        DateTime? deletedOn = null) =>
        new()
        {
            ToDoId = Guid.NewGuid(),
            ProfileId = profileId,
            Title = "todo",
            CreatedOn = createdOn,
            CompletedOn = completedOn,
            DeletedOn = deletedOn
        };

    [Fact]
    public async Task Handle_NoToDos_ReturnsZero()
    {
        var profileId = Guid.NewGuid();
        var mockContext = MockCommitmentsDbContextFactory.Create();

        var handler = new GetOutstandingToDoCountHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetOutstandingToDoCountRequest { ProfileId = profileId, AsOf = null, IncludeToday = false },
            CancellationToken.None);

        response.Count.Should().Be(0);
        response.TodayCount.Should().BeNull();
    }

    [Fact]
    public async Task Handle_OutstandingToDos_AreCountedAtAsOf()
    {
        var profileId = Guid.NewGuid();
        var asOf = new DateTimeOffset(2026, 4, 15, 12, 0, 0, TimeSpan.Zero);
        var todos = new List<ToDo>
        {
            MakeToDo(profileId, asOf.UtcDateTime.AddDays(-3)),
            MakeToDo(profileId, asOf.UtcDateTime.AddDays(-2)),
            MakeToDo(profileId, asOf.UtcDateTime.AddDays(-1))
        };

        var mockContext = MockCommitmentsDbContextFactory.Create();
        mockContext.Setup(c => c.Todos)
            .Returns(MockDbSetFactory.CreateMockDbSet(todos).Object);

        var handler = new GetOutstandingToDoCountHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetOutstandingToDoCountRequest { ProfileId = profileId, AsOf = asOf, IncludeToday = false },
            CancellationToken.None);

        response.Count.Should().Be(3);
    }

    [Fact]
    public async Task Handle_ToDoCreatedAfterAsOf_NotCounted()
    {
        var profileId = Guid.NewGuid();
        var asOf = new DateTimeOffset(2026, 4, 15, 12, 0, 0, TimeSpan.Zero);
        var todos = new List<ToDo>
        {
            MakeToDo(profileId, asOf.UtcDateTime.AddDays(1))
        };

        var mockContext = MockCommitmentsDbContextFactory.Create();
        mockContext.Setup(c => c.Todos)
            .Returns(MockDbSetFactory.CreateMockDbSet(todos).Object);

        var handler = new GetOutstandingToDoCountHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetOutstandingToDoCountRequest { ProfileId = profileId, AsOf = asOf },
            CancellationToken.None);

        response.Count.Should().Be(0);
    }

    [Fact]
    public async Task Handle_ToDoCompletedBeforeAsOf_NotCounted()
    {
        var profileId = Guid.NewGuid();
        var asOf = new DateTimeOffset(2026, 4, 15, 12, 0, 0, TimeSpan.Zero);
        var todos = new List<ToDo>
        {
            MakeToDo(profileId,
                createdOn: asOf.UtcDateTime.AddDays(-5),
                completedOn: asOf.UtcDateTime.AddDays(-1))
        };

        var mockContext = MockCommitmentsDbContextFactory.Create();
        mockContext.Setup(c => c.Todos)
            .Returns(MockDbSetFactory.CreateMockDbSet(todos).Object);

        var handler = new GetOutstandingToDoCountHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetOutstandingToDoCountRequest { ProfileId = profileId, AsOf = asOf },
            CancellationToken.None);

        response.Count.Should().Be(0);
    }

    [Fact]
    public async Task Handle_ToDoCompletedAfterAsOf_IsCountedAsOutstanding()
    {
        var profileId = Guid.NewGuid();
        var asOf = new DateTimeOffset(2026, 4, 15, 12, 0, 0, TimeSpan.Zero);
        var todos = new List<ToDo>
        {
            MakeToDo(profileId,
                createdOn: asOf.UtcDateTime.AddDays(-5),
                completedOn: asOf.UtcDateTime.AddDays(2))
        };

        var mockContext = MockCommitmentsDbContextFactory.Create();
        mockContext.Setup(c => c.Todos)
            .Returns(MockDbSetFactory.CreateMockDbSet(todos).Object);

        var handler = new GetOutstandingToDoCountHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetOutstandingToDoCountRequest { ProfileId = profileId, AsOf = asOf },
            CancellationToken.None);

        response.Count.Should().Be(1);
    }

    [Fact]
    public async Task Handle_ToDoDeletedBeforeAsOf_NotCounted()
    {
        var profileId = Guid.NewGuid();
        var asOf = new DateTimeOffset(2026, 4, 15, 12, 0, 0, TimeSpan.Zero);
        var todos = new List<ToDo>
        {
            MakeToDo(profileId,
                createdOn: asOf.UtcDateTime.AddDays(-5),
                deletedOn: asOf.UtcDateTime.AddDays(-1))
        };

        var mockContext = MockCommitmentsDbContextFactory.Create();
        mockContext.Setup(c => c.Todos)
            .Returns(MockDbSetFactory.CreateMockDbSet(todos).Object);

        var handler = new GetOutstandingToDoCountHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetOutstandingToDoCountRequest { ProfileId = profileId, AsOf = asOf },
            CancellationToken.None);

        response.Count.Should().Be(0);
    }

    [Fact]
    public async Task Handle_OtherProfileToDos_NotCounted()
    {
        var profileId = Guid.NewGuid();
        var otherProfileId = Guid.NewGuid();
        var asOf = new DateTimeOffset(2026, 4, 15, 12, 0, 0, TimeSpan.Zero);
        var todos = new List<ToDo>
        {
            MakeToDo(profileId, asOf.UtcDateTime.AddDays(-1)),
            MakeToDo(otherProfileId, asOf.UtcDateTime.AddDays(-1)),
            MakeToDo(otherProfileId, asOf.UtcDateTime.AddDays(-1))
        };

        var mockContext = MockCommitmentsDbContextFactory.Create();
        mockContext.Setup(c => c.Todos)
            .Returns(MockDbSetFactory.CreateMockDbSet(todos).Object);

        var handler = new GetOutstandingToDoCountHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetOutstandingToDoCountRequest { ProfileId = profileId, AsOf = asOf },
            CancellationToken.None);

        response.Count.Should().Be(1);
    }

    [Fact]
    public async Task Handle_IncludeTodayTrue_PopulatesTodayCount()
    {
        var profileId = Guid.NewGuid();
        var asOf = new DateTimeOffset(2026, 4, 15, 12, 0, 0, TimeSpan.Zero);
        var todos = new List<ToDo>
        {
            // outstanding at asOf and today
            MakeToDo(profileId, asOf.UtcDateTime.AddDays(-5)),
            // outstanding at asOf, completed after asOf but before today: counted at asOf, not today
            MakeToDo(profileId,
                createdOn: asOf.UtcDateTime.AddDays(-5),
                completedOn: asOf.UtcDateTime.AddDays(1))
        };

        var mockContext = MockCommitmentsDbContextFactory.Create();
        mockContext.Setup(c => c.Todos)
            .Returns(MockDbSetFactory.CreateMockDbSet(todos).Object);

        var handler = new GetOutstandingToDoCountHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetOutstandingToDoCountRequest { ProfileId = profileId, AsOf = asOf, IncludeToday = true },
            CancellationToken.None);

        response.Count.Should().Be(2);
        response.TodayCount.Should().Be(1);
    }

    [Fact]
    public async Task Handle_NullAsOf_ModeIsLiveAndAsOfIsCurrentUtc()
    {
        var profileId = Guid.NewGuid();
        var mockContext = MockCommitmentsDbContextFactory.Create();

        var handler = new GetOutstandingToDoCountHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetOutstandingToDoCountRequest { ProfileId = profileId, AsOf = null },
            CancellationToken.None);

        response.Mode.Should().Be("live");
        response.AsOf.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task Handle_AsOfProvided_ModeIsReview()
    {
        var profileId = Guid.NewGuid();
        var asOf = new DateTimeOffset(2026, 4, 15, 12, 0, 0, TimeSpan.Zero);
        var mockContext = MockCommitmentsDbContextFactory.Create();

        var handler = new GetOutstandingToDoCountHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetOutstandingToDoCountRequest { ProfileId = profileId, AsOf = asOf },
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

        var handler = new GetOutstandingToDoCountHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetOutstandingToDoCountRequest { ProfileId = profileId, AsOf = future },
            CancellationToken.None);

        response.AsOf.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));
    }
}
