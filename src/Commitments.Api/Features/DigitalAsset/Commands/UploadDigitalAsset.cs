// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core;
using Commitments.Core.Model.DigitalAssetAggregate;
using MediatR;
using DigitalAssetEntity = Commitments.Core.Model.DigitalAssetAggregate.DigitalAsset;

namespace Commitments.Api.Features.DigitalAsset;

public class UploadDigitalAssetRequest : IRequest<UploadDigitalAssetResponse>
{
    public byte[] Bytes { get; set; } = Array.Empty<byte>();
    public string? FileName { get; set; }
    public string? ContentType { get; set; }
}

public class UploadDigitalAssetResponse
{
    public Guid DigitalAssetId { get; set; }
}

public class UploadDigitalAssetRequestHandler : IRequestHandler<UploadDigitalAssetRequest, UploadDigitalAssetResponse>
{
    private readonly ICommitmentsDbContext _context;

    public UploadDigitalAssetRequestHandler(ICommitmentsDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<UploadDigitalAssetResponse> Handle(UploadDigitalAssetRequest request, CancellationToken cancellationToken)
    {
        var entity = new DigitalAssetEntity
        {
            DigitalAssetId = Guid.NewGuid(),
            Bytes = request.Bytes,
            ContentType = request.ContentType,
            Name = request.FileName
        };

        _context.DigitalAssets.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return new UploadDigitalAssetResponse
        {
            DigitalAssetId = entity.DigitalAssetId
        };
    }
}
