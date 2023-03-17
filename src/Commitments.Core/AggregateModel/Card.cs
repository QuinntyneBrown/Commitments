
namespace Commitments.Core.AggregateModel;

public class Card: BaseEntity
{
    public int CardId { get; set; }           
    public string Name { get; set; }
    public string Description { get; set; }
}
