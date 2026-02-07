// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Api.Controllers;
using Commitments.Core.Model.ActivityAggregate;
using Commitments.Api.Features.Activity;
using Commitments.Api.Features.Activity;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using Xunit;

namespace Commitments.Api.Tests.Controllers;

public class ActivityControllerTests
{
    private readonly Mock<ISender> _mockSender;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly ActivityController _controller;
    private readonly Guid _profileId = Guid.NewGuid();

    public ActivityControllerTests()
    {
        _mockSender = new Mock<ISender>();
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["ProfileId"] = _profileId.ToString();
        _mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

        _controller = new ActivityController(_mockHttpContextAccessor.Object, _mockSender.Object);
    }

    [Fact]
    public async Task Save_ShouldReturnSaveActivityResponse()
    {
        // Arrange
        var request = new SaveActivityRequest
        {
            Activity = new ActivityDto
            {
                ActivityId = Guid.NewGuid(),
                BehaviourId = Guid.NewGuid(),
                PerformedOn = DateTime.UtcNow
            }
        };
        var expectedResponse = new SaveActivityResponse
        {
            ActivityId = request.Activity.ActivityId
        };
        _mockSender.Setup(s => s.Send(It.IsAny<SaveActivityRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Save(request);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.ActivityId.Should().Be(expectedResponse.ActivityId);
    }

    [Fact]
    public async Task Save_ShouldSetProfileIdFromHttpContext()
    {
        // Arrange
        var request = new SaveActivityRequest
        {
            Activity = new ActivityDto
            {
                ActivityId = Guid.NewGuid(),
                BehaviourId = Guid.NewGuid(),
                PerformedOn = DateTime.UtcNow
            }
        };
        var expectedResponse = new SaveActivityResponse
        {
            ActivityId = request.Activity.ActivityId
        };
        _mockSender.Setup(s => s.Send(It.IsAny<SaveActivityRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        await _controller.Save(request);

        // Assert
        request.Activity.ProfileId.Should().Be(_profileId);
    }

    [Fact]
    public async Task Remove_ShouldCallSender()
    {
        // Arrange
        var request = new RemoveActivityRequest
        {
            ActivityId = Guid.NewGuid()
        };
        _mockSender.Setup(s => s.Send(It.IsAny<RemoveActivityRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _controller.Remove(request);

        // Assert
        _mockSender.Verify(s => s.Send(It.Is<RemoveActivityRequest>(r => r.ActivityId == request.ActivityId), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_ShouldReturnActivity()
    {
        // Arrange
        var activityId = Guid.NewGuid();
        var request = new GetActivityByIdRequest { ActivityId = activityId };
        var expectedResponse = new GetActivityByIdResponse
        {
            Activity = new ActivityDto { ActivityId = activityId }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<GetActivityByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetById(request);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.Activity.ActivityId.Should().Be(activityId);
    }

    [Fact]
    public async Task Get_ShouldReturnAllActivities()
    {
        // Arrange
        var expectedResponse = new GetActivitiesResponse
        {
            Activities = new List<ActivityDto>
            {
                new ActivityDto { ActivityId = Guid.NewGuid() },
                new ActivityDto { ActivityId = Guid.NewGuid() }
            }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<GetActivitiesRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Get();

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.Activities.Should().HaveCount(2);
    }
}
