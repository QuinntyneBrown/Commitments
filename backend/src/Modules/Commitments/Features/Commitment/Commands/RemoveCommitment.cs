// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Shared;
using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;


namespace Commitments.Features.Commitment;

public class RemoveCommitmentCommandValidator : AbstractValidator<RemoveCommitmentRequest>
{
    public RemoveCommitmentCommandValidator()
    {
        RuleFor(request => request.CommitmentId).NotEqual(default(Guid));
    }
}

public class RemoveCommitmentRequest : IRequest
{
    public Guid CommitmentId { get; set; }
}

public class RemoveCommitmentCommandHandler : IRequestHandler<RemoveCommitmentRequest>
{
    private readonly ICommitmentsDbContext _context;
    private readonly IEventBus _bus;

    public RemoveCommitmentCommandHandler(ICommitmentsDbContext context, IEventBus bus)
    {
        _context = context;
        _bus = bus;
    }

    public async Task Handle(RemoveCommitmentRequest request, CancellationToken cancellationToken)
    {
        var commitment = await _context.Commitments.FindAsync(request.CommitmentId);
        _context.Commitments.Remove(commitment);
        await _context.SaveChangesAsync(cancellationToken);

        await _bus.PublishAsync(new CommitmentChangedEvent
        {
            CommitmentId = commitment.CommitmentId,
            ProfileId = commitment.ProfileId,
            Kind = ChangeKind.Removed
        }, cancellationToken);
    }
}
