using Commitments.Core.Model.ActivityAggregate;
using Commitments.Core.Model.BehaviourAggregate;
using Commitments.Core.Model.BehaviourTypeAggregate;
using Commitments.Core.Model.CommitmentAggregate;
using Commitments.Core.Model.FrequencyAggregate;
using Commitments.Core.Model.FrequencyTypeAggregate;

namespace Commitments.Testing.Common.Builders;

public class ActivityBuilder
{
    private Guid _activityId = Guid.NewGuid();
    private Guid _profileId = Guid.NewGuid();
    private Guid _behaviourId = Guid.NewGuid();
    private DateTime _performedOn = DateTime.UtcNow;
    private string _description = "Test Activity";
    private Behaviour? _behaviour;

    public ActivityBuilder WithActivityId(Guid id) { _activityId = id; return this; }
    public ActivityBuilder WithProfileId(Guid id) { _profileId = id; return this; }
    public ActivityBuilder WithBehaviourId(Guid id) { _behaviourId = id; return this; }
    public ActivityBuilder WithPerformedOn(DateTime date) { _performedOn = date; return this; }
    public ActivityBuilder WithDescription(string desc) { _description = desc; return this; }
    public ActivityBuilder WithBehaviour(Behaviour b) { _behaviour = b; _behaviourId = b.BehaviourId; return this; }

    public Activity Build() => new Activity
    {
        ActivityId = _activityId,
        ProfileId = _profileId,
        BehaviourId = _behaviourId,
        PerformedOn = _performedOn,
        Description = _description,
        Behaviour = _behaviour!
    };
}

public class BehaviourBuilder
{
    private Guid _behaviourId = Guid.NewGuid();
    private string _name = "Test Behaviour";
    private string _slug = "test-behaviour";
    private string _description = "A test behaviour";
    private Guid _behaviourTypeId = Guid.NewGuid();
    private BehaviourType? _behaviourType;

    public BehaviourBuilder WithBehaviourId(Guid id) { _behaviourId = id; return this; }
    public BehaviourBuilder WithName(string name) { _name = name; return this; }
    public BehaviourBuilder WithSlug(string slug) { _slug = slug; return this; }
    public BehaviourBuilder WithDescription(string desc) { _description = desc; return this; }
    public BehaviourBuilder WithBehaviourTypeId(Guid id) { _behaviourTypeId = id; return this; }
    public BehaviourBuilder WithBehaviourType(BehaviourType bt) { _behaviourType = bt; _behaviourTypeId = bt.BehaviourTypeId; return this; }

    public Behaviour Build() => new Behaviour
    {
        BehaviourId = _behaviourId,
        Name = _name,
        Slug = _slug,
        Description = _description,
        BehaviourTypeId = _behaviourTypeId,
        BehaviourType = _behaviourType!
    };
}

public class BehaviourTypeBuilder
{
    private Guid _behaviourTypeId = Guid.NewGuid();
    private string _name = "Test BehaviourType";

    public BehaviourTypeBuilder WithBehaviourTypeId(Guid id) { _behaviourTypeId = id; return this; }
    public BehaviourTypeBuilder WithName(string name) { _name = name; return this; }

    public BehaviourType Build() => new BehaviourType
    {
        BehaviourTypeId = _behaviourTypeId,
        Name = _name
    };
}

public class CommitmentBuilder
{
    private Guid _commitmentId = Guid.NewGuid();
    private Guid _behaviourId = Guid.NewGuid();
    private Guid _profileId = Guid.NewGuid();
    private Behaviour? _behaviour;

    public CommitmentBuilder WithCommitmentId(Guid id) { _commitmentId = id; return this; }
    public CommitmentBuilder WithBehaviourId(Guid id) { _behaviourId = id; return this; }
    public CommitmentBuilder WithProfileId(Guid id) { _profileId = id; return this; }
    public CommitmentBuilder WithBehaviour(Behaviour b) { _behaviour = b; _behaviourId = b.BehaviourId; return this; }

    public Commitment Build() => new Commitment
    {
        CommitmentId = _commitmentId,
        BehaviourId = _behaviourId,
        ProfileId = _profileId,
        Behaviour = _behaviour!
    };
}

public class FrequencyBuilder
{
    private Guid _frequencyId = Guid.NewGuid();
    private Guid _frequencyTypeId = Guid.NewGuid();
    private bool _isDesirable = true;
    private FrequencyType? _frequencyType;

    public FrequencyBuilder WithFrequencyId(Guid id) { _frequencyId = id; return this; }
    public FrequencyBuilder WithFrequencyTypeId(Guid id) { _frequencyTypeId = id; return this; }
    public FrequencyBuilder WithIsDesirable(bool v) { _isDesirable = v; return this; }
    public FrequencyBuilder WithFrequencyType(FrequencyType ft) { _frequencyType = ft; _frequencyTypeId = ft.FrequencyTypeId; return this; }

    public Frequency Build() => new Frequency
    {
        FrequencyId = _frequencyId,
        FrequencyTypeId = _frequencyTypeId,
        IsDesirable = _isDesirable,
        FrequencyType = _frequencyType!
    };
}

public class FrequencyTypeBuilder
{
    private Guid _frequencyTypeId = Guid.NewGuid();
    private string _name = "Daily";

    public FrequencyTypeBuilder WithFrequencyTypeId(Guid id) { _frequencyTypeId = id; return this; }
    public FrequencyTypeBuilder WithName(string name) { _name = name; return this; }

    public FrequencyType Build() => new FrequencyType
    {
        FrequencyTypeId = _frequencyTypeId,
        Name = _name
    };
}
