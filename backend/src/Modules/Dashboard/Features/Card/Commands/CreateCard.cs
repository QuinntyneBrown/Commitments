using Commitments.Shared;
using Dashboard.Data;
using CardModel = Dashboard.Domain.CardAggregate.Card;
using MediatR;

namespace Dashboard.Features.Card;

public class CreateCardRequest : IRequest<CreateCardResponse>
{
    public CardDto Card { get; set; }
}

public class CreateCardResponse : ResponseBase
{
    public CardDto Card { get; set; }
}

public class CreateCardRequestHandler : IRequestHandler<CreateCardRequest, CreateCardResponse>
{
    private readonly IDashboardDbContext _context;

    public CreateCardRequestHandler(IDashboardDbContext context) => _context = context;

    public async Task<CreateCardResponse> Handle(CreateCardRequest request, CancellationToken cancellationToken)
    {
        var card = new CardModel
        {
            Name = request.Card.Name,
            Description = request.Card.Description
        };

        _context.Cards.Add(card);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateCardResponse { Card = card.ToDto() };
    }
}
