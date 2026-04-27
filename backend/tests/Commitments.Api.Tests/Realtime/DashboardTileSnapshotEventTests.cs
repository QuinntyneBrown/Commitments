using Commitments.Api.Realtime;
using FluentAssertions;
using Xunit;

namespace Commitments.Api.Tests.Realtime;

public class DashboardTileSnapshotEventTests
{
    [Fact]
    public void DefinesEventNamesForFiveOptInTiles_FromIcdSection6_3()
    {
        DashboardTileSnapshotEvent.DailyResultsUpdated.Should().Be("dailyResultsUpdated");
        DashboardTileSnapshotEvent.WeeklyFocusUpdated.Should().Be("weeklyFocusUpdated");
        DashboardTileSnapshotEvent.MonthlyProgressUpdated.Should().Be("monthlyProgressUpdated");
        DashboardTileSnapshotEvent.OutstandingTodosUpdated.Should().Be("outstandingTodosUpdated");
        DashboardTileSnapshotEvent.RelationsSummaryUpdated.Should().Be("relationsSummaryUpdated");
    }

    [Fact]
    public void All_ListsAllFiveEventNames()
    {
        DashboardTileSnapshotEvent.All.Should().BeEquivalentTo(new[]
        {
            "dailyResultsUpdated",
            "weeklyFocusUpdated",
            "monthlyProgressUpdated",
            "outstandingTodosUpdated",
            "relationsSummaryUpdated"
        });
    }
}
