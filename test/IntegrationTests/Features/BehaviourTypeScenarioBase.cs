namespace IntegrationTests.Features
{
    public class BehaviourTypeScenarioBase: ScenarioBase
    {
        public static class Get
        {
            public static string BehaviourTypes = "api/behaviourTypes";

            public static string BehaviourTypeById(int id)
            {
                return $"api/behaviourTypes/{id}";
            }
        }

        public static class Post
        {
            public static string BehaviourTypes = "api/behaviourTypes";
        }

        public static class Delete
        {
            public static string BehaviourType(int id)
            {
                return $"api/behaviourTypes/{id}";
            }
        }
    }
}
