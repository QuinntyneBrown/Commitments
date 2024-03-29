// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.AggregateModel;


namespace Commitments.Core.AggregateModel.ActivityAggregate.Commands;

public class RemoveActivityCommandValidator : AbstractValidator<RemoveActivityRequest>
{
    public RemoveActivityCommandValidator()
    {
        RuleFor(request => request.ActivityId).NotEqual(default(Guid));
    }
}

public class RemoveActivityRequest : IRequest
{
    public Guid ActivityId { get; set; }
}

public class RemoveActivityCommandHandler : IRequestHandler<RemoveActivityRequest>
{
    public ICommitmentsDbContext _context { get; set; }

    public RemoveActivityCommandHandler(ICommitmentsDbContext context) => _context = context;

    public async Task Handle(RemoveActivityRequest request, CancellationToken cancellationToken)
    {
        _context.Activities.Remove(await _context.Activities.FindAsync(request.ActivityId));
        await _context.SaveChangesAsync(cancellationToken);
    }
}