// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.AggregateModel;
using Commitments.Core.Interfaces;


namespace Commitments.Core.AggregateModel.CardLayoutAggregate.Commands;

public class RemoveCardLayoutCommandValidator : AbstractValidator<RemoveCardLayoutRequest>
{
    public RemoveCardLayoutCommandValidator()
    {
        RuleFor(request => request.CardLayoutId).NotEqual(default(Guid));
    }
}

public class RemoveCardLayoutRequest : IRequest
{
    public Guid CardLayoutId { get; set; }
}

public class RemoveCardLayoutCommandHandler : IRequestHandler<RemoveCardLayoutRequest>
{
    public ICommimentsDbContext _context { get; set; }


    public async Task Handle(RemoveCardLayoutRequest request, CancellationToken cancellationToken)
    {
        _context.CardLayouts.Remove(await _context.CardLayouts.FindAsync(request.CardLayoutId));
        await _context.SaveChangesAsync(cancellationToken);
    }

}

