using Commitments.Core.Entities;


namespace Commitments.Api.Features.CardLayouts;

public class CardLayoutApiModel
{        
    public int CardLayoutId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public static CardLayoutApiModel FromCardLayout(CardLayout cardLayout)
    {
        var model = new CardLayoutApiModel();
        model.CardLayoutId = cardLayout.CardLayoutId;
        model.Name = cardLayout.Name;
        model.Description = cardLayout.Description;
        return model;
    }
}
