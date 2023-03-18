// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;


namespace Commitments.Core.AggregateModel.CommitmentAggregate.Commands;

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
    public ICommitmentsDbContext _context { get; set; }

    public RemoveCommitmentCommandHandler(ICommitmentsDbContext context) => _context = context;

    public async Task Handle(RemoveCommitmentRequest request, CancellationToken cancellationToken)
    {
        _context.Commitments.Remove(await _context.Commitments.FindAsync(request.CommitmentId));
        await _context.SaveChangesAsync(cancellationToken);
    }

}

