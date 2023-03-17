// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


namespace IntegrationTests.Features;

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

