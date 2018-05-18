namespace IntegrationTests.Features
{
    public class CommitmentFrequencyScenarioBase: ScenarioBase
    {
        public static class Get
        {
            public static string CommitmentFrequencies = "api/commitmentFrequencies";

            public static string CommitmentFrequencyById(int id)
            {
                return $"api/commitmentFrequencies/{id}";
            }
        }

        public static class Post
        {
            public static string CommitmentFrequencies = "api/commitmentFrequencies";
        }

        public static class Delete
        {
            public static string CommitmentFrequency(int id)
            {
                return $"api/commitmentFrequencies/{id}";
            }
        }
    }
}
