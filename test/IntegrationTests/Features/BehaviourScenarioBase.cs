// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


namespace IntegrationTests.Features;

public class BehaviourScenarioBase: ScenarioBase
{
    public static class Get
    {
        public static string Behaviours = "api/behaviours";

        public static string BehaviourById(int id)
        {
            return $"api/behaviours/{id}";
        }
    }

    public static class Post
    {
        public static string Behaviours = "api/behaviours";
    }

    public static class Delete
    {
        public static string Behaviour(int id)
        {
            return $"api/behaviours/{id}";
        }
    }

}

