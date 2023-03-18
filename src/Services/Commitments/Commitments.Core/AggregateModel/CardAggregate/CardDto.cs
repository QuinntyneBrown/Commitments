// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel.CardAggregate;

namespace Commitments.Core.AggregateModel.CardAggregate;

public class CardDto
{
    public Guid CardId { get; set; }
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

