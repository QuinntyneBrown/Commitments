namespace Commitments.Api.Realtime;

public sealed record GoalProgressUpdatedPayload(
    Guid GoalId,
    Guid? BehaviourId,
    int Count,
    int Target,
    int Percent,
    DateTimeOffset AsOf,
    DateOnly Date,
    string Reason,
    Guid? SourceActivityId);
