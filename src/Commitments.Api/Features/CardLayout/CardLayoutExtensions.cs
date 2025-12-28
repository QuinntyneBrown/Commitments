// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Model.CardLayoutAggregate;
using Microsoft.EntityFrameworkCore;
using CardLayoutEntity = Commitments.Core.Model.CardLayoutAggregate.CardLayout;

namespace Commitments.Api.Features.CardLayout;

public static class CardLayoutExtensions
{
    public static CardLayoutDto ToDto(this CardLayoutEntity cardLayout)
    {
        return new CardLayoutDto
        {
            CardLayoutId = cardLayout.CardLayoutId,
            Name = cardLayout.Name,
            Description = cardLayout.Description,
        };

    }

    public async static Task<List<CardLayoutDto>> ToDtosAsync(this IQueryable<CardLayoutEntity> cardLayouts, CancellationToken cancellationToken)
    {
        return await cardLayouts.Select(x => x.ToDto()).ToListAsync(cancellationToken);
    }

}