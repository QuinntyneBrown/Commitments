namespace Commitments.Api.Realtime;

public sealed record DashboardTileDataInvalidatedPayload(
    IReadOnlyList<string> Datasets,
    IReadOnlyList<Guid> AffectedGoalIds,
    IReadOnlyList<Guid> AffectedCommitmentIds,
    IReadOnlyList<Guid> AffectedToDoIds,
    DateOnly? From,
    DateOnly? To,
    string Reason);

public static class DashboardTileDataset
{
    public const string DailyResults = "dailyResults";
    public const string WeeklyFocus = "weeklyFocus";
    public const string MonthlyProgress = "monthlyProgress";
    public const string OutstandingTodos = "outstandingTodos";
    public const string Relations = "relations";
    public const string GoalTrend = "goalTrend";

    public static readonly IReadOnlyList<string> All = new[]
    {
        DailyResults, WeeklyFocus, MonthlyProgress, OutstandingTodos, Relations, GoalTrend
    };

    public static readonly IReadOnlyList<string> AllExceptToDos = new[]
    {
        DailyResults, WeeklyFocus, MonthlyProgress, Relations, GoalTrend
    };
}
