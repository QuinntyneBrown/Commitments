// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace DashboardService.Core.AggregateModel.CardAggregate.Queries;

public class GetCardsRequest : IRequest<GetCardsResponse> { }

public class GetCardsResponse : ResponseBase
{
    public List<CardDto> Cards { get; set; }
}


public class GetCardsRequestHandler : IRequestHandler<GetCardsRequest, GetCardsResponse>
{
    private readonly ILogger<GetCardsRequestHandler> _logger;

    private readonly IDashboardServiceDbContext _context;

    public GetCardsRequestHandler(ILogger<GetCardsRequestHandler> logger, IDashboardServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetCardsResponse> Handle(GetCardsRequest request, CancellationToken cancellationToken)
    {
        return new()
        {
            Cards = await _context.Cards.AsNoTracking().ToDtosAsync(cancellationToken)
        };

    }

}



