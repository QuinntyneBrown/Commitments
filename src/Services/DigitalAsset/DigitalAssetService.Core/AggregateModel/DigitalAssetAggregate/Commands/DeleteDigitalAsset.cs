// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace DigitalAssetService.Core.AggregateModel.DigitalAssetAggregate.Commands;

public class DeleteDigitalAssetRequestValidator : AbstractValidator<DeleteDigitalAssetRequest> { }

public class DeleteDigitalAssetRequest : IRequest<DeleteDigitalAssetResponse>
{
    public Guid DigitalAssetId { get; set; }
}


public class DeleteDigitalAssetResponse : ResponseBase
{
    public DigitalAssetDto DigitalAsset { get; set; }
}


public class DeleteDigitalAssetRequestHandler : IRequestHandler<DeleteDigitalAssetRequest, DeleteDigitalAssetResponse>
{
    private readonly ILogger<DeleteDigitalAssetRequestHandler> _logger;

    private readonly IDigitalAssetServiceDbContext _context;

    public DeleteDigitalAssetRequestHandler(ILogger<DeleteDigitalAssetRequestHandler> logger, IDigitalAssetServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<DeleteDigitalAssetResponse> Handle(DeleteDigitalAssetRequest request, CancellationToken cancellationToken)
    {
        var digitalAsset = await _context.DigitalAssets.FindAsync(request.DigitalAssetId);

        _context.DigitalAssets.Remove(digitalAsset);

        await _context.SaveChangesAsync(cancellationToken);

        return new()
        {
            DigitalAsset = digitalAsset.ToDto()
        };
    }

}