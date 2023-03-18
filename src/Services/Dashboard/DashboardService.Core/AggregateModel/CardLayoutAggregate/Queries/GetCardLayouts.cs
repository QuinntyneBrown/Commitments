// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace DashboardService.Core.AggregateModel.CardLayoutAggregate.Queries;

public class GetCardLayoutsRequest : IRequest<GetCardLayoutsResponse> { }

public class GetCardLayoutsResponse : ResponseBase
{
    public List<CardLayoutDto> CardLayouts { get; set; }
}


public class GetCardLayoutsRequestHandler : IRequestHandler<GetCardLayoutsRequest, GetCardLayoutsResponse>
{
    private readonly ILogger<GetCardLayoutsRequestHandler> _logger;

    private readonly IDashboardServiceDbContext _context;

    public GetCardLayoutsRequestHandler(ILogger<GetCardLayoutsRequestHandler> logger, IDashboardServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetCardLayoutsResponse> Handle(GetCardLayoutsRequest request, CancellationToken cancellationToken)
    {
        return new()
        {
            CardLayouts = await _context.CardLayouts.AsNoTracking().ToDtosAsync(cancellationToken)
        };

    }

}



