// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace DashboardService.Core.AggregateModel.CardLayoutAggregate;

public static class CardLayoutExtensions
{
    public static CardLayoutDto ToDto(this CardLayout cardLayout)
    {
        return new CardLayoutDto
        {
            CardLayoutId = cardLayout.CardLayoutId,
            Name = cardLayout.Name,
            Description = cardLayout.Description,
        };

    }

    public async static Task<List<CardLayoutDto>> ToDtosAsync(this IQueryable<CardLayout> cardLayouts, CancellationToken cancellationToken)
    {
        return await cardLayouts.Select(x => x.ToDto()).ToListAsync(cancellationToken);
    }

}