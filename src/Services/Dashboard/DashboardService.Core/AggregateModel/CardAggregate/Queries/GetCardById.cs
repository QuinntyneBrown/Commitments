// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace DashboardService.Core.AggregateModel.CardAggregate.Queries;

public class GetCardByIdRequest : IRequest<GetCardByIdResponse>
{
    public Guid CardId { get; set; }
}


public class GetCardByIdResponse : ResponseBase
{
    public CardDto Card { get; set; }
}


public class GetCardByIdRequestHandler : IRequestHandler<GetCardByIdRequest, GetCardByIdResponse>
{
    private readonly ILogger<GetCardByIdRequestHandler> _logger;

    private readonly IDashboardServiceDbContext _context;

    public GetCardByIdRequestHandler(ILogger<GetCardByIdRequestHandler> logger, IDashboardServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetCardByIdResponse> Handle(GetCardByIdRequest request, CancellationToken cancellationToken)
    {
        return new()
        {
            Card = (await _context.Cards.AsNoTracking().SingleOrDefaultAsync(x => x.CardId == request.CardId)).ToDto()
        };

    }

}