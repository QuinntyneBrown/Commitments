// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Controllers;
using Commitments.Features.ToDo;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Controllers;

public class ToDoControllerTests
{
    private readonly Mock<ISender> _mockSender = new();
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor = new();
    private readonly ToDoController _controller;
    private readonly Guid _profileId = Guid.NewGuid();

    public ToDoControllerTests()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["ProfileId"] = _profileId.ToString();
        _mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

        _controller = new ToDoController(_mockHttpContextAccessor.Object, _mockSender.Object);
    }

    [Fact]
    public async Task GetOutstandingCount_ShouldDispatchWithProfileIdAsOfAndIncludeToday()
    {
        var asOf = new DateTimeOffset(2026, 4, 15, 12, 0, 0, TimeSpan.Zero);
        GetOutstandingToDoCountRequest? captured = null;
        _mockSender
            .Setup(s => s.Send(It.IsAny<GetOutstandingToDoCountRequest>(), It.IsAny<CancellationToken>()))
            .Callback<object, CancellationToken>((r, _) => captured = (GetOutstandingToDoCountRequest)r)
            .ReturnsAsync(new GetOutstandingToDoCountResponse());

        await _controller.GetOutstandingCount(asOf, includeToday: true);

        captured!.ProfileId.Should().Be(_profileId);
        captured!.AsOf.Should().Be(asOf);
        captured!.IncludeToday.Should().BeTrue();
    }

    [Fact]
    public async Task GetOutstandingCount_DefaultsAreNullAsOfAndIncludeTodayFalse()
    {
        GetOutstandingToDoCountRequest? captured = null;
        _mockSender
            .Setup(s => s.Send(It.IsAny<GetOutstandingToDoCountRequest>(), It.IsAny<CancellationToken>()))
            .Callback<object, CancellationToken>((r, _) => captured = (GetOutstandingToDoCountRequest)r)
            .ReturnsAsync(new GetOutstandingToDoCountResponse());

        await _controller.GetOutstandingCount(asOf: null, includeToday: false);

        captured!.AsOf.Should().BeNull();
        captured!.IncludeToday.Should().BeFalse();
    }
}
