// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace DigitalAssetService.Core.AggregateModel.DigitalAssetAggregate.Queries;

public class GetDigitalAssetByIdRequest : IRequest<GetDigitalAssetByIdResponse>
{
    public Guid DigitalAssetId { get; set; }
}


public class GetDigitalAssetByIdResponse : ResponseBase
{
    public DigitalAssetDto DigitalAsset { get; set; }
}


public class GetDigitalAssetByIdRequestHandler : IRequestHandler<GetDigitalAssetByIdRequest, GetDigitalAssetByIdResponse>
{
    private readonly ILogger<GetDigitalAssetByIdRequestHandler> _logger;

    private readonly IDigitalAssetServiceDbContext _context;

    public GetDigitalAssetByIdRequestHandler(ILogger<GetDigitalAssetByIdRequestHandler> logger, IDigitalAssetServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetDigitalAssetByIdResponse> Handle(GetDigitalAssetByIdRequest request, CancellationToken cancellationToken)
    {
        return new()
        {
            DigitalAsset = (await _context.DigitalAssets.AsNoTracking().SingleOrDefaultAsync(x => x.DigitalAssetId == request.DigitalAssetId)).ToDto()
        };

    }

}