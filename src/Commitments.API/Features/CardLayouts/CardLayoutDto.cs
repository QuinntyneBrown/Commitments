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
