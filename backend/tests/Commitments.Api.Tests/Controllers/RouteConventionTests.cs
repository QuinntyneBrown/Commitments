using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Commitments.Api.Tests.Controllers;

/// <summary>
/// Verifies that every controller uses api/v{version} routes that match frontend service URLs (bug-195).
/// </summary>
public class RouteConventionTests
{
    private static string? GetRoute(Type controllerType)
    {
        var attr = controllerType.GetCustomAttributes(typeof(RouteAttribute), inherit: true)
            .Cast<RouteAttribute>()
            .FirstOrDefault();
        return attr?.Template;
    }

    [Theory]
    [InlineData(typeof(Identity.Controllers.UserController), "api/v{version:apiVersion}/users")]
    [InlineData(typeof(Commitments.Controllers.BehaviourController), "api/v{version:apiVersion}/behaviours")]
    [InlineData(typeof(Commitments.Controllers.ActivityController), "api/v{version:apiVersion}/activities")]
    [InlineData(typeof(Commitments.Controllers.CommitmentController), "api/v{version:apiVersion}/commitments")]
    [InlineData(typeof(Commitments.Controllers.NoteController), "api/v{version:apiVersion}/notes")]
    [InlineData(typeof(Commitments.Controllers.GoalProgressController), "api/v{version:apiVersion}/goal-progress")]
    [InlineData(typeof(Commitments.Controllers.WeeklyFocusController), "api/v{version:apiVersion}/weekly-focus")]
    [InlineData(typeof(Commitments.Controllers.MonthlyProgressController), "api/v{version:apiVersion}/monthly-progress")]
    [InlineData(typeof(Commitments.Controllers.RelationsController), "api/v{version:apiVersion}/relations")]
    [InlineData(typeof(Commitments.Controllers.ToDoController), "api/v{version:apiVersion}/toDos")]
    public void Controller_Route_Starts_With_V_Version_Prefix(Type controllerType, string expectedRoute)
    {
        var actual = GetRoute(controllerType);
        actual.Should().Be(expectedRoute, $"{controllerType.Name} must expose route '{expectedRoute}' to match frontend service URL");
    }
}
