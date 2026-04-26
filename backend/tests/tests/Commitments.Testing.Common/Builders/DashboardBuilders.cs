using Dashboard.Core.Model.DashboardAggregate;
using Dashboard.Core.Model.DashboardCardAggregate;
using Dashboard.Core.Model.CardAggregate;
using Dashboard.Core.Model.CardLayoutAggregate;

namespace Commitments.Testing.Common.Builders;

public class DashboardEntityBuilder
{
    private Guid _dashboardId = Guid.NewGuid();
    private string _name = "Test Dashboard";
    private Guid? _profileId = Guid.NewGuid();

    public DashboardEntityBuilder WithDashboardId(Guid id) { _dashboardId = id; return this; }
    public DashboardEntityBuilder WithName(string name) { _name = name; return this; }
    public DashboardEntityBuilder WithProfileId(Guid? id) { _profileId = id; return this; }

    public DashboardEntity Build()
    {
        var d = new DashboardEntity(_name, _profileId);
        d.DashboardId = _dashboardId;
        return d;
    }
}

public class CardBuilder
{
    private Guid _cardId = Guid.NewGuid();
    private string _name = "Test Card";
    private string _description = "A test card";

    public CardBuilder WithCardId(Guid id) { _cardId = id; return this; }
    public CardBuilder WithName(string name) { _name = name; return this; }
    public CardBuilder WithDescription(string desc) { _description = desc; return this; }

    public Card Build() => new Card
    {
        CardId = _cardId,
        Name = _name,
        Description = _description
    };
}

public class CardLayoutBuilder
{
    private Guid _cardLayoutId = Guid.NewGuid();
    private string _name = "Test Layout";
    private string _description = "A test layout";

    public CardLayoutBuilder WithCardLayoutId(Guid id) { _cardLayoutId = id; return this; }
    public CardLayoutBuilder WithName(string name) { _name = name; return this; }
    public CardLayoutBuilder WithDescription(string desc) { _description = desc; return this; }

    public CardLayout Build()
    {
        var cl = new CardLayout(_name, _description);
        cl.CardLayoutId = _cardLayoutId;
        return cl;
    }
}

public class DashboardCardBuilder
{
    private Guid _dashboardCardId = Guid.NewGuid();
    private Guid _dashboardId = Guid.NewGuid();
    private Guid _cardId = Guid.NewGuid();
    private Guid _cardLayoutId = Guid.NewGuid();

    public DashboardCardBuilder WithDashboardCardId(Guid id) { _dashboardCardId = id; return this; }
    public DashboardCardBuilder WithDashboardId(Guid id) { _dashboardId = id; return this; }
    public DashboardCardBuilder WithCardId(Guid id) { _cardId = id; return this; }
    public DashboardCardBuilder WithCardLayoutId(Guid id) { _cardLayoutId = id; return this; }

    public DashboardCard Build() => new DashboardCard
    {
        DashboardCardId = _dashboardCardId,
        DashboardId = _dashboardId,
        CardId = _cardId,
        CardLayoutId = _cardLayoutId
    };
}
