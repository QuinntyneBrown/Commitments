// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


namespace IntegrationTests.Features;

public class FrequencyTypeScenarioBase: ScenarioBase
{
    public static class Get
    {
        public static string FrequencyTypes = "api/frequencyTypes";

        public static string FrequencyTypeById(int id)
        {
            return $"api/frequencyTypes/{id}";
        }
    }

    public static class Post
    {
        public static string FrequencyTypes = "api/frequencyTypes";
    }

    public static class Delete
    {
        public static string FrequencyType(int id)
        {
            return $"api/frequencyTypes/{id}";
        }
    }
}

