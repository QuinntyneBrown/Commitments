// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


namespace IntegrationTests.Features;

public class ToDoScenarioBase: ScenarioBase
{
    public static class Get
    {
        public static string ToDos = "api/toDos";

        public static string ToDoById(int id)
        {
            return $"api/toDos/{id}";
        }
    }

    public static class Post
    {
        public static string ToDos = "api/toDos";
    }

    public static class Delete
    {
        public static string ToDo(int id)
        {
            return $"api/toDos/{id}";
        }
    }
}

