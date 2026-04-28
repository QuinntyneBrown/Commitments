using Commitments.Data;
using Commitments.Domain.ActivityAggregate;
using Commitments.Domain.BehaviourAggregate;
using Commitments.Domain.BehaviourTypeAggregate;
using Commitments.Features.Activity;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Queries;

public class GetActivitiesSortedTests
{
    [Fact]
    public async Task Handle_ReturnsActivitiesOrderedByPerformedOnDescending(/* design 10 Slice A */)
    {
        var options = new DbContextOptionsBuilder<CommitmentsDbContext>()
            .UseInMemoryDatabase($"sort-{Guid.NewGuid()}")
            .Options;

        await using var ctx = new CommitmentsDbContext(options);
        var bt = new BehaviourType { BehaviourTypeId = Guid.NewGuid(), Name = "x" };
        var b = new Behaviour { BehaviourId = Guid.NewGuid(), Name = "y", Slug = "y", Description = "y", BehaviourTypeId = bt.BehaviourTypeId };
        ctx.BehaviourTypes.Add(bt);
        ctx.Behaviours.Add(b);

        var oldId = Guid.NewGuid();
        var newId = Guid.NewGuid();
        var midId = Guid.NewGuid();
        ctx.Activities.Add(new Activity { ActivityId = oldId, BehaviourId = b.BehaviourId, Description = "x", PerformedOn = new DateTime(2026, 1, 1) });
        ctx.Activities.Add(new Activity { ActivityId = newId, BehaviourId = b.BehaviourId, Description = "x", PerformedOn = new DateTime(2026, 6, 1) });
        ctx.Activities.Add(new Activity { ActivityId = midId, BehaviourId = b.BehaviourId, Description = "x", PerformedOn = new DateTime(2026, 3, 1) });
        await ctx.SaveChangesAsync(CancellationToken.None);

        var handler = new GetActivitiesQueryHandler(ctx);
        var response = await handler.Handle(new GetActivitiesRequest(), CancellationToken.None);

        var ids = response.Activities.Select(a => a.ActivityId).ToList();
        ids.Should().Equal(newId, midId, oldId);
    }
}
