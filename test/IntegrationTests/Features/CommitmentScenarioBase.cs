namespace IntegrationTests.Features
{
    public class CommitmentScenarioBase: ScenarioBase
    {
        public static class Get
        {
            public static string Commitments = "api/commitments";

            public static string CommitmentById(int id)
            {
                return $"api/commitments/{id}";
            }
        }

        public static class Post
        {
            public static string Commitments = "api/commitments";
        }

        public static class Delete
        {
            public static string Commitment(int id)
            {
                return $"api/commitments/{id}";
            }
        }
    }
}
