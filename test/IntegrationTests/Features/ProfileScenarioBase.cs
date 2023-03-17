// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


namespace IntegrationTests.Features;

public class ProfileScenarioBase: ScenarioBase
{
    public static class Get
    {
        public static string Profiles = "api/profiles";

        public static string ProfileById(int id)
        {
            return $"api/profiles/{id}";
        }
    }

    public static class Post
    {
        public static string Save = "api/profiles";
        public static string Create = "api/profiles/create";
    }

    public static class Delete
    {
        public static string Profile(int id)
        {
            return $"api/profiles/{id}";
        }
    }
}

