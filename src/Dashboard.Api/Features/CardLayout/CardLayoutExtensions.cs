using Microsoft.EntityFrameworkCore;
using CardLayoutModel = Dashboard.Core.Model.CardLayoutAggregate.CardLayout;

namespace Dashboard.Api.Features.CardLayout;

public static class CardLayoutExtensions
{
    public static CardLayoutDto ToDto(this CardLayoutModel cardLayout)
    {
        return new CardLayoutDto
        {
            CardLayoutId = cardLayout.CardLayoutId,
            Name = cardLayout.Name,
            Description = cardLayout.Description
        };
    }

    public static async Task<List<CardLayoutDto>> ToDtosAsync(this IQueryable<CardLayoutModel> cardLayouts, CancellationToken cancellationToken)
    {
        return await cardLayouts.Select(x => x.ToDto()).ToListAsync(cancellationToken);
    }
}
