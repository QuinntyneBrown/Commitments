using Microsoft.EntityFrameworkCore;
using CardModel = Dashboard.Core.Model.CardAggregate.Card;

namespace Dashboard.Api.Features.Card;

public static class CardExtensions
{
    public static CardDto ToDto(this CardModel card)
    {
        return new CardDto
        {
            CardId = card.CardId,
            Name = card.Name,
            Description = card.Description
        };
    }

    public static async Task<List<CardDto>> ToDtosAsync(this IQueryable<CardModel> cards, CancellationToken cancellationToken)
    {
        return await cards.Select(x => x.ToDto()).ToListAsync(cancellationToken);
    }
}
