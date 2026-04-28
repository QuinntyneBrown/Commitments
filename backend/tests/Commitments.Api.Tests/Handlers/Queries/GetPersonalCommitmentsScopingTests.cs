using Commitments.Data;
using Commitments.Domain.BehaviourAggregate;
using Commitments.Domain.BehaviourTypeAggregate;
using Commitments.Domain.CommitmentAggregate;
using Commitments.Features.Commitment;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using CommitmentEntity = Commitments.Domain.CommitmentAggregate.Commitment;

namespace Commitments.Api.Tests.Handlers.Queries;

public class GetPersonalCommitmentsScopingTests
{
    [Fact]
    public async Task Handle_ReturnsOnlyCallerProfileCommitments(/* design 09 Slice E / L2-010 AC#1 */)
    {
        var caller = Guid.NewGuid();
        var other = Guid.NewGuid();

        var options = new DbContextOptionsBuilder<CommitmentsDbContext>()
            .UseInMemoryDatabase($"scoping-{Guid.NewGuid()}")
            .Options;

        await using var ctx = new CommitmentsDbContext(options);
        var bt = new BehaviourType { BehaviourTypeId = Guid.NewGuid(), Name = "x" };
        var b = new Behaviour { BehaviourId = Guid.NewGuid(), Name = "y", BehaviourTypeId = bt.BehaviourTypeId };
        ctx.BehaviourTypes.Add(bt);
        ctx.Behaviours.Add(b);
        ctx.Commitments.Add(new CommitmentEntity { CommitmentId = Guid.NewGuid(), ProfileId = caller, BehaviourId = b.BehaviourId });
        ctx.Commitments.Add(new CommitmentEntity { CommitmentId = Guid.NewGuid(), ProfileId = caller, BehaviourId = b.BehaviourId });
        ctx.Commitments.Add(new CommitmentEntity { CommitmentId = Guid.NewGuid(), ProfileId = other,  BehaviourId = b.BehaviourId });
        await ctx.SaveChangesAsync(CancellationToken.None);

        var handler = new GetPersonalCommitmentsQueryHandler(ctx);

        var response = await handler.Handle(
            new GetPersonalCommitmentsRequest { ProfileId = caller },
            CancellationToken.None);

        response.Commitments.Should().HaveCount(2);
        response.Commitments.Should().OnlyContain(c => c.ProfileId == caller);
    }
}
