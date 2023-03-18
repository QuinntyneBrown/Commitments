// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace DigitalAssetService.Core.AggregateModel.DigitalAssetAggregate.Queries;

public class GetDigitalAssetsPageRequest : IRequest<GetDigitalAssetsPageResponse>
{
    public int PageSize { get; set; }
    public int Index { get; set; }
}


public class GetDigitalAssetsPageResponse : ResponseBase
{
    public int Length { get; set; }
    public List<DigitalAssetDto> Entities { get; set; }
}


public class GetDigitalAssetsPageRequestHandler : IRequestHandler<GetDigitalAssetsPageRequest, GetDigitalAssetsPageResponse>
{
    private readonly ILogger<GetDigitalAssetsPageRequestHandler> _logger;

    private readonly IDigitalAssetServiceDbContext _context;

    public GetDigitalAssetsPageRequestHandler(ILogger<GetDigitalAssetsPageRequestHandler> logger, IDigitalAssetServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetDigitalAssetsPageResponse> Handle(GetDigitalAssetsPageRequest request, CancellationToken cancellationToken)
    {
        var query = from digitalAsset in _context.DigitalAssets
                    select digitalAsset;

        var length = await _context.DigitalAssets.AsNoTracking().CountAsync();

        var digitalAssets = await query.Page(request.Index, request.PageSize).AsNoTracking()
            .Select(x => x.ToDto()).ToListAsync();

        return new()
        {
            Length = length,
            Entities = digitalAssets
        };

    }

}



