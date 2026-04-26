using Commitments.Shared;
using Dashboard.Core;
using MediatR;

namespace Dashboard.Api.Features.Card;

public class UpdateCardRequest : IRequest<UpdateCardResponse>
{
    public CardDto Card { get; set; }
}

public class UpdateCardResponse : ResponseBase
{
    public CardDto Card { get; set; }
}

public class UpdateCardRequestHandler : IRequestHandler<UpdateCardRequest, UpdateCardResponse>
{
    private readonly IDashboardDbContext _context;

    public UpdateCardRequestHandler(IDashboardDbContext context) => _context = context;

    public async Task<UpdateCardResponse> Handle(UpdateCardRequest request, CancellationToken cancellationToken)
    {
        var card = await _context.Cards.FindAsync(request.Card.CardId);
        if (card == null) return new UpdateCardResponse();

        card.Name = request.Card.Name;
        card.Description = request.Card.Description;

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateCardResponse { Card = card.ToDto() };
    }
}
