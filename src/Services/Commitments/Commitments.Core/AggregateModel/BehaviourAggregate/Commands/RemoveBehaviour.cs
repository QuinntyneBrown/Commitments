// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.AggregateModel;
using Commitments.Core.Interfaces;


namespace Commitments.Core.AggregateModel.BehaviourAggregate.Commands;

public class RemoveBehaviourCommandValidator : AbstractValidator<RemoveBehaviourRequest>
{
    public RemoveBehaviourCommandValidator()
    {
        RuleFor(request => request.BehaviourId).NotEqual(default(Guid));
    }
}

public class RemoveBehaviourRequest : IRequest
{
    public Guid BehaviourId { get; set; }
}

public class RemoveBehaviourCommandHandler : IRequestHandler<RemoveBehaviourRequest>
{
    public ICommitmentsDbContext _context { get; set; }

    public RemoveBehaviourCommandHandler(ICommitmentsDbContext context) => _context = context;

    public async Task Handle(RemoveBehaviourRequest request, CancellationToken cancellationToken)
    {
        _context.Behaviours.Remove(await _context.Behaviours.FindAsync(request.BehaviourId));
        await _context.SaveChangesAsync(cancellationToken);
    }

}

