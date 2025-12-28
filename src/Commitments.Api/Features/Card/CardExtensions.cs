// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Model.CardAggregate;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CardEntity = Commitments.Core.Model.CardAggregate.Card;

namespace Commitments.Api.Features.Card;

public static class CardExtensions
{
    public static CardDto ToDto(this CardEntity card)
    {
        return new CardDto
        {
            CardId = card.CardId,
            Name = card.Name,
            Description = card.Description,
        };

    }

    public async static Task<List<CardDto>> ToDtosAsync(this IQueryable<CardEntity> cards, CancellationToken cancellationToken)
    {
        return await cards.Select(x => x.ToDto()).ToListAsync(cancellationToken);
    }

}