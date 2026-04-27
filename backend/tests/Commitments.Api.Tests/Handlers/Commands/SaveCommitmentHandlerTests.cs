using Commitments.Features.Commitment;
using Commitments.Data;
using Commitments.Domain.CommitmentAggregate;
using Commitments.Shared;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Commands;

public class SaveCommitmentHandlerTests : IDisposable
{
    private readonly Mock<IEventBus> _bus = new();
    private readonly CommitmentsDbContext _ctx;

    public SaveCommitmentHandlerTests()
    {
        var options = new DbContextOptionsBuilder<CommitmentsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _ctx = new CommitmentsDbContext(options);
    }

    [Fact]
    public async Task Handle_NewCommitment_PublishesCommitmentChangedEvent_WithCreatedKind()
    {
        var handler = new SaveCommitmentCommandHandler(_ctx, _bus.Object);
        var profileId = Guid.NewGuid();
        var request = new SaveCommitmentRequest
        {
            Commitment = new CommitmentDto
            {
                CommitmentId = Guid.NewGuid(),
                BehaviourId = Guid.NewGuid(),
                ProfileId = profileId,
                CommitmentFrequencies = new List<CommitmentFrequencyDto>()
            }
        };

        await handler.Handle(request, CancellationToken.None);

        _bus.Verify(b => b.PublishAsync(
            It.Is<CommitmentChangedEvent>(e =>
                e.ProfileId == profileId &&
                e.Kind == ChangeKind.Created),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ExistingCommitment_PublishesUpdatedKind()
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

        var handler = new SaveCommitmentCommandHandler(_ctx, _bus.Object);
        var request = new SaveCommitmentRequest
        {
            Commitment = new CommitmentDto
            {
                CommitmentId = commitmentId,
                BehaviourId = Guid.NewGuid(),
                ProfileId = profileId,
                CommitmentFrequencies = new List<CommitmentFrequencyDto>()
            }
        };

        await handler.Handle(request, CancellationToken.None);

        _bus.Verify(b => b.PublishAsync(
            It.Is<CommitmentChangedEvent>(e => e.Kind == ChangeKind.Updated),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    public void Dispose() => _ctx.Dispose();
}
