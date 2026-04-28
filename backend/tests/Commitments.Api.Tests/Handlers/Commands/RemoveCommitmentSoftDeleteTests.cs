using Commitments.Data;
using Commitments.Domain.CommitmentAggregate;
using Commitments.Features.Commitment;
using Commitments.Shared;
using Commitments.Testing.Common;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using CommitmentEntity = Commitments.Domain.CommitmentAggregate.Commitment;

namespace Commitments.Api.Tests.Handlers.Commands;

public class RemoveCommitmentSoftDeleteTests
{
    [Fact]
    public async Task Handle_SetsIsDeletedTrue_DoesNotPhysicallyDelete(/* design 09 Slice D / L2-011 AC#1 */)
    {
        var commitmentId = Guid.NewGuid();
        var profileId = Guid.NewGuid();

        var options = new DbContextOptionsBuilder<CommitmentsDbContext>()
            .UseInMemoryDatabase($"soft-delete-{Guid.NewGuid()}")
            .Options;

        await using var ctx = new CommitmentsDbContext(options);
        ctx.Commitments.Add(new CommitmentEntity { CommitmentId = commitmentId, ProfileId = profileId });
        await ctx.SaveChangesAsync(CancellationToken.None);

        var handler = new RemoveCommitmentCommandHandler(ctx, new NullEventBus());

        await handler.Handle(new RemoveCommitmentRequest { CommitmentId = commitmentId }, CancellationToken.None);

        var raw = await ctx.Commitments.IgnoreQueryFilters().SingleAsync(c => c.CommitmentId == commitmentId);
        raw.IsDeleted.Should().BeTrue();
        (await ctx.Commitments.AnyAsync(c => c.CommitmentId == commitmentId)).Should().BeFalse();
    }
}
