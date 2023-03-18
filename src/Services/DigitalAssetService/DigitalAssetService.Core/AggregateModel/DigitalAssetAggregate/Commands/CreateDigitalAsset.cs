// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace DigitalAssetService.Core.AggregateModel.DigitalAssetAggregate.Commands;

public class CreateDigitalAssetRequestValidator : AbstractValidator<CreateDigitalAssetRequest> { }

public class CreateDigitalAssetRequest : IRequest<CreateDigitalAssetResponse> { }

public class CreateDigitalAssetResponse : ResponseBase
{
    public DigitalAssetDto DigitalAsset { get; set; }
}


public class CreateDigitalAssetRequestHandler : IRequestHandler<CreateDigitalAssetRequest, CreateDigitalAssetResponse>
{
    private readonly ILogger<CreateDigitalAssetRequestHandler> _logger;

    private readonly IDigitalAssetServiceDbContext _context;

    public CreateDigitalAssetRequestHandler(ILogger<CreateDigitalAssetRequestHandler> logger, IDigitalAssetServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<CreateDigitalAssetResponse> Handle(CreateDigitalAssetRequest request, CancellationToken cancellationToken)
    {
        var digitalAsset = new DigitalAsset();

        _context.DigitalAssets.Add(digitalAsset);

        await _context.SaveChangesAsync(cancellationToken);

        return new()
        {
            DigitalAsset = digitalAsset.ToDto()
        };

    }

}



