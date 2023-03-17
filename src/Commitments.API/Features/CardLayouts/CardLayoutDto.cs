// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel;


namespace Commitments.Api.Features.CardLayouts;

public class CardLayoutDto
{        
    public int CardLayoutId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public static CardLayoutDto FromCardLayout(CardLayout cardLayout)
    {
        var model = new CardLayoutDto();
        model.CardLayoutId = cardLayout.CardLayoutId;
        model.Name = cardLayout.Name;
        model.Description = cardLayout.Description;
        return model;
    }
}

