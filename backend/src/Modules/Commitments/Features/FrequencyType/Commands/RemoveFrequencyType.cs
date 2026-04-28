// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;


namespace Commitments.Features.FrequencyType;

public class RemoveFrequencyTypeCommandValidator : AbstractValidator<RemoveFrequencyTypeRequest>
{
    public RemoveFrequencyTypeCommandValidator()
    {
        RuleFor(request => request.FrequencyTypeId).NotEqual(default(Guid));
    }
}

public class RemoveFrequencyTypeRequest : IRequest
{
    public Guid FrequencyTypeId { get; set; }
}

public class RemoveFrequencyTypeCommandHandler : IRequestHandler<RemoveFrequencyTypeRequest>
{
    public ICommitmentsDbContext _context { get; set; }

    public RemoveFrequencyTypeCommandHandler(ICommitmentsDbContext context) => _context = context;

    public async Task Handle(RemoveFrequencyTypeRequest request, CancellationToken cancellationToken)
    {
        var referenceCount = _context.Frequencies.Count(f => f.FrequencyTypeId == request.FrequencyTypeId);
        if (referenceCount > 0)
        {
            throw new BadHttpRequestException($"Cannot delete: referenced by {referenceCount} frequency(ies)", 400);
        }

        _context.FrequencyTypes.Remove(await _context.FrequencyTypes.FindAsync(request.FrequencyTypeId));
        await _context.SaveChangesAsync(cancellationToken);
    }

}