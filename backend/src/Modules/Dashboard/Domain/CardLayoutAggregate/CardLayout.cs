namespace Dashboard.Domain.CardLayoutAggregate;

public class CardLayout
{
    public CardLayout(string name, string description)
    {
        Name = name;
        Description = description;
    }
    public Guid CardLayoutId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
