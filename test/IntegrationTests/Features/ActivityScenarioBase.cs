namespace IntegrationTests.Features
{
    public class ActivityScenarioBase: ScenarioBase
    {
        public static class Get
        {
            public static string Activities = "api/activities";

            public static string ActivityById(int id)
            {
                return $"api/activities/{id}";
            }
        }

        public static class Post
        {
            public static string Activities = "api/activities";
        }

        public static class Delete
        {
            public static string Activity(int id)
            {
                return $"api/activities/{id}";
            }
        }
    }
}
