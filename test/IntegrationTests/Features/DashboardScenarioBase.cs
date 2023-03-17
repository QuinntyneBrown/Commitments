// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


namespace IntegrationTests.Features;

public class DashboardScenarioBase: ScenarioBase
{
    public static class Get
    {
        public static string Dashboards = "api/dashboards";

        public static string DashboardById(int id)
        {
            return $"api/dashboards/{id}";
        }
    }

    public static class Post
    {
        public static string Dashboards = "api/dashboards";
    }

    public static class Delete
    {
        public static string Dashboard(int id)
        {
            return $"api/dashboards/{id}";
        }
    }
}

