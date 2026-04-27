// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Controllers;
using Commitments.Features.WeeklyFocus;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Controllers;

public class WeeklyFocusControllerTests
{
    private readonly Mock<ISender> _mockSender = new();
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor = new();
    private readonly WeeklyFocusController _controller;
    private readonly Guid _profileId = Guid.NewGuid();

    public WeeklyFocusControllerTests()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["ProfileId"] = _profileId.ToString();
        _mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

        _controller = new WeeklyFocusController(_mockHttpContextAccessor.Object, _mockSender.Object);
    }

    [Fact]
    public async Task Get_ShouldDispatchWithProfileIdAndAsOf()
    {
        var asOf = new DateTimeOffset(2026, 4, 22, 12, 0, 0, TimeSpan.Zero);
        GetWeeklyFocusRequest? captured = null;
        _mockSender
            .Setup(s => s.Send(It.IsAny<GetWeeklyFocusRequest>(), It.IsAny<CancellationToken>()))
            .Callback<object, CancellationToken>((r, _) => captured = (GetWeeklyFocusRequest)r)
            .ReturnsAsync(new GetWeeklyFocusResponse());

        await _controller.Get(asOf);

        captured!.ProfileId.Should().Be(_profileId);
        captured!.AsOf.Should().Be(asOf);
    }

    [Fact]
    public async Task Get_NoAsOf_DispatchesNullAsOf()
    {
        GetWeeklyFocusRequest? captured = null;
        _mockSender
            .Setup(s => s.Send(It.IsAny<GetWeeklyFocusRequest>(), It.IsAny<CancellationToken>()))
            .Callback<object, CancellationToken>((r, _) => captured = (GetWeeklyFocusRequest)r)
            .ReturnsAsync(new GetWeeklyFocusResponse());

        await _controller.Get(asOf: null);

        captured!.AsOf.Should().BeNull();
    }
}
