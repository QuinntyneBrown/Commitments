namespace IntegrationTests.Features
{
    public class FrequencyScenarioBase: ScenarioBase
    {
        public static class Get
        {
            public static string Frequencies = "api/frequencies";

            public static string FrequencyById(int id)
            {
                return $"api/frequencies/{id}";
            }
        }

        public static class Post
        {
            public static string Frequencies = "api/frequencies";
        }

        public static class Delete
        {
            public static string Frequency(int id)
            {
                return $"api/frequencies/{id}";
            }
        }
    }
}
