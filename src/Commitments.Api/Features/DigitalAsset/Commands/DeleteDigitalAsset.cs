// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Commitments.Api.Features.DigitalAsset;

public class DeleteDigitalAssetRequest : IRequest<DeleteDigitalAssetResponse>
{
    public Guid DigitalAssetId { get; set; }
}

public class DeleteDigitalAssetResponse
{
    public Guid DigitalAssetId { get; set; }
}

public class DeleteDigitalAssetRequestHandler : IRequestHandler<DeleteDigitalAssetRequest, DeleteDigitalAssetResponse>
{
    private readonly ICommitmentsDbContext _context;

    public DeleteDigitalAssetRequestHandler(ICommitmentsDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<DeleteDigitalAssetResponse> Handle(DeleteDigitalAssetRequest request, CancellationToken cancellationToken)
    {
        var entity = await _context.DigitalAssets.FirstOrDefaultAsync(x => x.DigitalAssetId == request.DigitalAssetId, cancellationToken);

        if (entity is not null)
        {
            _context.DigitalAssets.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return new DeleteDigitalAssetResponse
        {
            DigitalAssetId = request.DigitalAssetId
        };
    }
}
