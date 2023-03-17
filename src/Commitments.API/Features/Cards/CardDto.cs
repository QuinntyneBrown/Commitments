// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel;


namespace Commitments.Api.Features.Cards;

public class CardDto
{        
    public int CardId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public static CardDto FromCard(Card card)
    {
        var model = new CardDto
        {
            CardId = card.CardId,
            Name = card.Name,
            Description = card.Description
        };
        return model;
    }
}

