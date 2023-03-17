// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


namespace IntegrationTests.Features;

public class CardLayoutScenarioBase: ScenarioBase
{
    public static class Get
    {
        public static string CardLayouts = "api/cardLayouts";

        public static string CardLayoutById(int id)
        {
            return $"api/cardLayouts/{id}";
        }
    }

    public static class Post
    {
        public static string CardLayouts = "api/cardLayouts";
    }

    public static class Delete
    {
        public static string CardLayout(int id)
        {
            return $"api/cardLayouts/{id}";
        }
    }
}

