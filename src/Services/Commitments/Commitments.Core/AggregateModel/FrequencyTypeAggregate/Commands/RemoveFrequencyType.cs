// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.AggregateModel;


namespace Commitments.Core.AggregateModel.FrequencyTypeAggregate.Commands;

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
        _context.FrequencyTypes.Remove(await _context.FrequencyTypes.FindAsync(request.FrequencyTypeId));
        await _context.SaveChangesAsync(cancellationToken);
    }

}

