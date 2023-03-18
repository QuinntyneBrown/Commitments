// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace DashboardService.Core.AggregateModel.CardLayoutAggregate.Queries;

public class GetCardLayoutByIdRequest : IRequest<GetCardLayoutByIdResponse>
{
    public Guid CardLayoutId { get; set; }
}


public class GetCardLayoutByIdResponse : ResponseBase
{
    public CardLayoutDto CardLayout { get; set; }
}


public class GetCardLayoutByIdRequestHandler : IRequestHandler<GetCardLayoutByIdRequest, GetCardLayoutByIdResponse>
{
    private readonly ILogger<GetCardLayoutByIdRequestHandler> _logger;

    private readonly IDashboardServiceDbContext _context;

    public GetCardLayoutByIdRequestHandler(ILogger<GetCardLayoutByIdRequestHandler> logger, IDashboardServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetCardLayoutByIdResponse> Handle(GetCardLayoutByIdRequest request, CancellationToken cancellationToken)
    {
        return new()
        {
            CardLayout = (await _context.CardLayouts.AsNoTracking().SingleOrDefaultAsync(x => x.CardLayoutId == request.CardLayoutId)).ToDto()
        };

    }

}



