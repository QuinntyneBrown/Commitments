// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


namespace IntegrationTests.Features;

public class CardScenarioBase: ScenarioBase
{
    public static class Get
    {
        public static string Cards = "api/cards";

        public static string CardById(int id)
        {
            return $"api/cards/{id}";
        }
    }

    public static class Post
    {
        public static string Cards = "api/cards";
    }

    public static class Delete
    {
        public static string Card(int id)
        {
            return $"api/cards/{id}";
        }
    }
}

