// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


namespace IntegrationTests.Features;

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

