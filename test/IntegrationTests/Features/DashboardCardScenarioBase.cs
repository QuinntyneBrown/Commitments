namespace IntegrationTests.Features
{
    public class DashboardCardScenarioBase: ScenarioBase
    {
        public static class Get
        {
            public static string DashboardCards = "api/dashboardCards";

            public static string DashboardCardById(int id)
            {
                return $"api/dashboardCards/{id}";
            }
        }

        public static class Post
        {
            public static string DashboardCards = "api/dashboardCards";
        }

        public static class Delete
        {
            public static string DashboardCard(int id)
            {
                return $"api/dashboardCards/{id}";
            }
        }
    }
}
