// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Interfaces;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;


namespace Commitments.Core.AggregateModel.BehaviourTypeAggregate.Commands;

public class RemoveBehaviourTypeCommandValidator : AbstractValidator<RemoveBehaviourTypeRequest>
{
    public RemoveBehaviourTypeCommandValidator()
    {
        RuleFor(request => request.BehaviourTypeId).NotEqual(default(Guid));
    }
}

public class RemoveBehaviourTypeRequest : IRequest
{
    public Guid BehaviourTypeId { get; set; }
}

public class RemoveBehaviourTypeCommandHandler : IRequestHandler<RemoveBehaviourTypeRequest>
{
    public ICommimentsDbContext _context { get; set; }

    public RemoveBehaviourTypeCommandHandler(ICommimentsDbContext context) => _context = context;

    public async Task Handle(RemoveBehaviourTypeRequest request, CancellationToken cancellationToken)
    {
        _context.BehaviourTypes.Remove(await _context.BehaviourTypes.FindAsync(request.BehaviourTypeId));
        await _context.SaveChangesAsync(cancellationToken);
    }

}

