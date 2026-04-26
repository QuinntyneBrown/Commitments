using Commitments.Shared;
using Dashboard.Core;
using MediatR;

namespace Dashboard.Api.Features.Card;

public class GetCardByIdRequest : IRequest<GetCardByIdResponse>
{
    public Guid CardId { get; set; }
}

public class GetCardByIdResponse : ResponseBase
{
    public CardDto? Card { get; set; }
}

public class GetCardByIdRequestHandler : IRequestHandler<GetCardByIdRequest, GetCardByIdResponse>
{
    private readonly IDashboardDbContext _context;

    public GetCardByIdRequestHandler(IDashboardDbContext context) => _context = context;

    public async Task<GetCardByIdResponse> Handle(GetCardByIdRequest request, CancellationToken cancellationToken)
    {
        var card = await _context.Cards.FindAsync(request.CardId);
        return new GetCardByIdResponse { Card = card?.ToDto() };
    }
}
