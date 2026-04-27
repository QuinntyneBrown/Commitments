// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Data;
using Commitments.Domain.CommitmentAggregate;
using Commitments.Shared;
using Microsoft.EntityFrameworkCore;
using CommitmentEntity = Commitments.Domain.CommitmentAggregate.Commitment;


namespace Commitments.Features.Commitment;

public class SaveCommitmentCommandValidator : AbstractValidator<SaveCommitmentRequest>
{
    public SaveCommitmentCommandValidator()
    {
        RuleFor(request => request.Commitment.CommitmentId).NotEmpty();
    }
}

public class SaveCommitmentRequest : IRequest<SaveCommitmentResponse>
{
    public CommitmentDto Commitment { get; set; }
}

public class SaveCommitmentResponse
{
    public Guid CommitmentId { get; set; }
}

public class SaveCommitmentCommandHandler : IRequestHandler<SaveCommitmentRequest, SaveCommitmentResponse>
{
    private readonly ICommitmentsDbContext _context;
    private readonly IEventBus _bus;

    public SaveCommitmentCommandHandler(ICommitmentsDbContext context, IEventBus bus)
    {
        _context = context;
        _bus = bus;
    }

    public async Task<SaveCommitmentResponse> Handle(SaveCommitmentRequest request, CancellationToken cancellationToken)
    {
        var commitment = await _context.Commitments
            .Include(x => x.CommitmentFrequencies)
            .Include("CommitmentFrequencies.Frequency")
            .SingleOrDefaultAsync(x => x.CommitmentId == request.Commitment.CommitmentId);

        var kind = commitment is null ? ChangeKind.Created : ChangeKind.Updated;

        if (commitment == null) _context.Commitments.Add(commitment = new CommitmentEntity());

        commitment.BehaviourId = request.Commitment.BehaviourId;
        commitment.ProfileId = request.Commitment.ProfileId;

        commitment.CommitmentFrequencies.Clear();

        foreach (var cf in request.Commitment.CommitmentFrequencies)
        {
            commitment.CommitmentFrequencies.Add(new CommitmentFrequency()
            {
                FrequencyId = cf.FrequencyId
            });
        }
        await _context.SaveChangesAsync(cancellationToken);

        await _bus.PublishAsync(new CommitmentChangedEvent
        {
            CommitmentId = commitment.CommitmentId,
            ProfileId = commitment.ProfileId,
            Kind = kind
        }, cancellationToken);

        return new SaveCommitmentResponse() { CommitmentId = commitment.CommitmentId };
    }
}
