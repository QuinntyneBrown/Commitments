// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.AggregateModel;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Core.AggregateModel.FrequencyAggregate.Commands;

public class SaveFrequencyCommandValidator : AbstractValidator<SaveFrequencyRequest>
{
    public SaveFrequencyCommandValidator()
    {
        RuleFor(request => request.Frequency.FrequencyId).NotNull();
    }
}

public class SaveFrequencyRequest : IRequest<SaveFrequencyResponse>
{
    public FrequencyDto Frequency { get; set; }
}

public class SaveFrequencyResponse
{
    public Guid FrequencyId { get; set; }
}

public class SaveFrequencyCommandHandler : IRequestHandler<SaveFrequencyRequest, SaveFrequencyResponse>
{
    public ICommitmentsDbContext _context { get; set; }

    public SaveFrequencyCommandHandler(ICommitmentsDbContext context) => _context = context;

    public async Task<SaveFrequencyResponse> Handle(SaveFrequencyRequest request, CancellationToken cancellationToken)
    {
        var frequency = await _context.Frequencies
            .Include(x => x.FrequencyType)
            .SingleOrDefaultAsync(x => x.FrequencyId == request.Frequency.FrequencyId);

        if (frequency == null) _context.Frequencies.Add(frequency = new Frequency());

        frequency.Frequency = request.Frequency.Frequency;

        frequency.FrequencyTypeId = request.Frequency.FrequencyTypeId;

        await _context.SaveChangesAsync(cancellationToken);

        return new SaveFrequencyResponse() { FrequencyId = frequency.FrequencyId };
    }
}

