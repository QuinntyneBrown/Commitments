namespace Commitments.Api.Realtime;

public static class DashboardTileSnapshotEvent
{
    public const string DailyResultsUpdated = "dailyResultsUpdated";
    public const string WeeklyFocusUpdated = "weeklyFocusUpdated";
    public const string MonthlyProgressUpdated = "monthlyProgressUpdated";
    public const string OutstandingTodosUpdated = "outstandingTodosUpdated";
    public const string RelationsSummaryUpdated = "relationsSummaryUpdated";

    public static readonly IReadOnlyList<string> All = new[]
    {
        DailyResultsUpdated,
        WeeklyFocusUpdated,
        MonthlyProgressUpdated,
        OutstandingTodosUpdated,
        RelationsSummaryUpdated
    };
}
