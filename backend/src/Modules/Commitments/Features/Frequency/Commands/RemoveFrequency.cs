// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;


namespace Commitments.Features.Frequency;

public class RemoveFrequencyCommandValidator : AbstractValidator<RemoveFrequencyRequest>
{
    public RemoveFrequencyCommandValidator()
    {
        RuleFor(request => request.FrequencyId).NotEqual(default(Guid));
    }
}

public class RemoveFrequencyRequest : IRequest
{
    public Guid FrequencyId { get; set; }
}

public class RemoveFrequencyCommandHandler : IRequestHandler<RemoveFrequencyRequest>
{
    public ICommitmentsDbContext _context { get; set; }

    public RemoveFrequencyCommandHandler(ICommitmentsDbContext context) => _context = context;

    public async Task Handle(RemoveFrequencyRequest request, CancellationToken cancellationToken)
    {
        var referenceCount = _context.CommitmentFrequencies.Count(cf => cf.FrequencyId == request.FrequencyId);
        if (referenceCount > 0)
        {
            throw new BadHttpRequestException($"Cannot delete: referenced by {referenceCount} commitment-frequency join(s)", 400);
        }

        _context.Frequencies.Remove(await _context.Frequencies.FindAsync(request.FrequencyId));
        await _context.SaveChangesAsync(cancellationToken);
    }

}