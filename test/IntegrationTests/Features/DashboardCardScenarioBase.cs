using System.Collections.Generic;
using System.Linq;


namespace IntegrationTests.Features;

public class DashboardCardScenarioBase: ScenarioBase
{
    public static class Get
    {
        public static string DashboardCards = "api/dashboardCards";

        public static string DashboardCardById(int id)
        {
            return $"api/dashboardCards/{id}";
        }

        public static string DashboardCardByIds(List<int> ids)
        {
            return $"api/dashboardCards/range?{string.Join("&", ids.Select(x => $"dashboardCardIds={x}"))}";
        }
    }

    public static class Post
    {
        public static string DashboardCards = "api/dashboardCards";
        public static string DashboardCardsRange = "api/dashboardCards/range";
    }

    public static class Delete
    {
        public static string DashboardCard(int id)
        {
            return $"api/dashboardCards/{id}";
        }
    }
}
