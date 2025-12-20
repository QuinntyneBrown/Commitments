// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Commitments.Api.Features.DigitalAsset;

public class UpdateDigitalAssetRequestValidator : AbstractValidator<UpdateDigitalAssetRequest>
{
    public UpdateDigitalAssetRequestValidator()
    {
        RuleFor(x => x.DigitalAssetId).NotEmpty();
    }
}

public class UpdateDigitalAssetRequest : IRequest<UpdateDigitalAssetResponse>
{
    public Guid DigitalAssetId { get; set; }
    public string? Name { get; set; }
    public string? ContentType { get; set; }
    public byte[]? Bytes { get; set; }
}

public class UpdateDigitalAssetResponse
{
    public Guid DigitalAssetId { get; set; }
}

public class UpdateDigitalAssetRequestHandler : IRequestHandler<UpdateDigitalAssetRequest, UpdateDigitalAssetResponse>
{
    private readonly ICommitmentsDbContext _context;

    public UpdateDigitalAssetRequestHandler(ICommitmentsDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<UpdateDigitalAssetResponse> Handle(UpdateDigitalAssetRequest request, CancellationToken cancellationToken)
    {
        var entity = await _context.DigitalAssets.FirstOrDefaultAsync(x => x.DigitalAssetId == request.DigitalAssetId, cancellationToken);

        if (entity is null)
        {
            throw new InvalidOperationException($"DigitalAsset {request.DigitalAssetId} not found");
        }

        entity.Name = request.Name ?? entity.Name;
        entity.ContentType = request.ContentType ?? entity.ContentType;
        entity.Bytes = request.Bytes ?? entity.Bytes;

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateDigitalAssetResponse
        {
            DigitalAssetId = entity.DigitalAssetId
        };
    }
}
