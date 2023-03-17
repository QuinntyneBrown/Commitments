// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


namespace IntegrationTests.Features;

public class UserScenarioBase: ScenarioBase
{
    public static class Post
    {
        public static string Token = "api/users/token";
        public static string ChangePassword = "api/users/changePassword";
    }        
}

