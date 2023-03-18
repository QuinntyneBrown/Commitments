// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace DigitalAssetService.Core.AggregateModel.DigitalAssetAggregate.Queries;

public class GetDigitalAssetsRequest : IRequest<GetDigitalAssetsResponse> { }

public class GetDigitalAssetsResponse : ResponseBase
{
    public List<DigitalAssetDto> DigitalAssets { get; set; }
}


public class GetDigitalAssetsRequestHandler : IRequestHandler<GetDigitalAssetsRequest, GetDigitalAssetsResponse>
{
    private readonly ILogger<GetDigitalAssetsRequestHandler> _logger;

    private readonly IDigitalAssetServiceDbContext _context;

    public GetDigitalAssetsRequestHandler(ILogger<GetDigitalAssetsRequestHandler> logger, IDigitalAssetServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetDigitalAssetsResponse> Handle(GetDigitalAssetsRequest request, CancellationToken cancellationToken)
    {
        return new()
        {
            //DigitalAssets = await _context.DigitalAssets.AsNoTracking().ToDtosAsync(cancellationToken)
        };

    }

}



