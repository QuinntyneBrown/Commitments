// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.AggregateModel;
using Commitments.Core.Interfaces;


namespace Commitments.Core.AggregateModel.DigitalAssetAggregate.Commands;

public class RemoveDigitalAssetCommandValidator : AbstractValidator<RemoveDigitalAssetRequest>
{
    public RemoveDigitalAssetCommandValidator()
    {
        RuleFor(request => request.DigitalAssetId).NotEqual(default(Guid));
    }
}

public class RemoveDigitalAssetRequest : IRequest
{
    public Guid DigitalAssetId { get; set; }
}

public class RemoveDigitalAssetCommandHandler : IRequestHandler<RemoveDigitalAssetRequest>
{
    public ICommimentsDbContext _context { get; set; }

    public RemoveDigitalAssetCommandHandler(ICommimentsDbContext context) => _context = context;

    public async Task Handle(RemoveDigitalAssetRequest request, CancellationToken cancellationToken)
    {
        _context.DigitalAssets.Remove(await _context.DigitalAssets.FindAsync(request.DigitalAssetId));
        await _context.SaveChangesAsync(cancellationToken);
    }

}

