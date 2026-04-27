using Commitments.Features.Commitment;
using Commitments.Data;
using Commitments.Domain.CommitmentAggregate;
using Commitments.Shared;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Commands;

public class RemoveCommitmentHandlerTests : IDisposable
{
    private readonly Mock<IEventBus> _bus = new();
    private readonly CommitmentsDbContext _ctx;

    public RemoveCommitmentHandlerTests()
    {
        var options = new DbContextOptionsBuilder<CommitmentsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _ctx = new CommitmentsDbContext(options);
    }

    [Fact]
    public async Task Handle_ExistingCommitment_PublishesCommitmentChangedEvent_WithRemovedKind()
    {
        var commitmentId = Guid.NewGuid();
        var profileId = Guid.NewGuid();
        _ctx.Commitments.Add(new Commitment
        {
            CommitmentId = commitmentId,
            BehaviourId = Guid.NewGuid(),
            ProfileId = profileId
        });
        _ctx.SaveChanges();

        var handler = new RemoveCommitmentCommandHandler(_ctx, _bus.Object);

        await handler.Handle(new RemoveCommitmentRequest { CommitmentId = commitmentId }, CancellationToken.None);

        _bus.Verify(b => b.PublishAsync(
            It.Is<CommitmentChangedEvent>(e =>
                e.CommitmentId == commitmentId &&
                e.ProfileId == profileId &&
                e.Kind == ChangeKind.Removed),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    public void Dispose() => _ctx.Dispose();
}
