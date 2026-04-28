// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;


namespace Commitments.Features.BehaviourType;

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
    public ICommitmentsDbContext _context { get; set; }

    public RemoveBehaviourTypeCommandHandler(ICommitmentsDbContext context) => _context = context;

    public async Task Handle(RemoveBehaviourTypeRequest request, CancellationToken cancellationToken)
    {
        var referenceCount = _context.Behaviours.Count(b => b.BehaviourTypeId == request.BehaviourTypeId);
        if (referenceCount > 0)
        {
            throw new BadHttpRequestException($"Cannot delete: referenced by {referenceCount} behaviour(s)", 400);
        }

        _context.BehaviourTypes.Remove(await _context.BehaviourTypes.FindAsync(request.BehaviourTypeId));
        await _context.SaveChangesAsync(cancellationToken);
    }

}