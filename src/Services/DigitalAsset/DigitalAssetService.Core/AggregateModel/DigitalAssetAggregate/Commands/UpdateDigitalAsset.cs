// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;
using Kernel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DigitalAssetService.Core.AggregateModel.DigitalAssetAggregate.Commands;

public class UpdateDigitalAssetRequestValidator : AbstractValidator<UpdateDigitalAssetRequest> { }

public class UpdateDigitalAssetRequest : IRequest<UpdateDigitalAssetResponse>
{
    public Guid DigitalAssetId { get; set; }
}

public class UpdateDigitalAssetResponse : ResponseBase
{
    public DigitalAssetDto DigitalAsset { get; set; }
}


public class UpdateDigitalAssetRequestHandler : IRequestHandler<UpdateDigitalAssetRequest, UpdateDigitalAssetResponse>
{
    private readonly ILogger<UpdateDigitalAssetRequestHandler> _logger;

    private readonly IDigitalAssetServiceDbContext _context;

    public UpdateDigitalAssetRequestHandler(ILogger<UpdateDigitalAssetRequestHandler> logger, IDigitalAssetServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<UpdateDigitalAssetResponse> Handle(UpdateDigitalAssetRequest request, CancellationToken cancellationToken)
    {
        var digitalAsset = await _context.DigitalAssets.SingleAsync(x => x.DigitalAssetId == request.DigitalAssetId);

        /*        digitalAsset.DigitalAssetId = request.DigitalAssetId;
                digitalAsset.Name = request.Name;
                digitalAsset.Bytes = request.Bytes;
                digitalAsset.ContentType = request.ContentType;
                digitalAsset.Height = request.Height;
                digitalAsset.Width = request.Width;*/

        await _context.SaveChangesAsync(cancellationToken);

        return new()
        {
            DigitalAsset = digitalAsset.ToDto()
        };

    }

}