using Commitments.Core.Entities;

namespace Commitments.Api.Features.Cards
{
    public class CardApiModel
    {        
        public int CardId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public static CardApiModel FromCard(Card card)
        {
            var model = new CardApiModel
            {
                CardId = card.CardId,
                Name = card.Name,
                Description = card.Description
            };
            return model;
        }
    }
}
