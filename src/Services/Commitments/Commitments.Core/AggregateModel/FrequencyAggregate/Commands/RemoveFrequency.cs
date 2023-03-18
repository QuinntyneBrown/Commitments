// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.AggregateModel;
using Commitments.Core.Interfaces;


namespace Commitments.Core.AggregateModel.FrequencyAggregate.Commands;

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
        _context.Frequencies.Remove(await _context.Frequencies.FindAsync(request.FrequencyId));
        await _context.SaveChangesAsync(cancellationToken);
    }

}

