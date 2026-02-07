using Commitments.Shared;
using Dashboard.Core;
using MediatR;

namespace Dashboard.Api.Features.Card;

public class GetCardsRequest : IRequest<GetCardsResponse> { }

public class GetCardsResponse : ResponseBase
{
    public required List<CardDto> Cards { get; set; }
}

public class GetCardsRequestHandler : IRequestHandler<GetCardsRequest, GetCardsResponse>
{
    private readonly IDashboardDbContext _context;

    public GetCardsRequestHandler(IDashboardDbContext context) => _context = context;

    public async Task<GetCardsResponse> Handle(GetCardsRequest request, CancellationToken cancellationToken)
    {
        return new GetCardsResponse
        {
            Cards = await _context.Cards.ToDtosAsync(cancellationToken)
        };
    }
}
