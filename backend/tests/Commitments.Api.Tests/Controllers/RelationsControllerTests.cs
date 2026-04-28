// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Controllers;
using Commitments.Features.Relations;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Controllers;

public class RelationsControllerTests
{
    private readonly Mock<ISender> _mockSender = new();
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor = new();
    private readonly RelationsController _controller;
    private readonly Guid _profileId = Guid.NewGuid();

    public RelationsControllerTests()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["ProfileId"] = _profileId.ToString();
        _mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

        _controller = new RelationsController(_mockHttpContextAccessor.Object, _mockSender.Object);
    }

    [Fact]
    public async Task GetSummary_ShouldDispatchWithProfileIdAsOfAndTop()
    {
        var asOf = new DateTimeOffset(2026, 4, 22, 12, 0, 0, TimeSpan.Zero);
        GetRelationsSummaryRequest? captured = null;
        _mockSender
            .Setup(s => s.Send(It.IsAny<GetRelationsSummaryRequest>(), It.IsAny<CancellationToken>()))
            .Callback<object, CancellationToken>((r, _) => captured = (GetRelationsSummaryRequest)r)
            .ReturnsAsync(new GetRelationsSummaryResponse());

        await _controller.GetSummary(asOf, top: 5);

        captured!.ProfileId.Should().Be(_profileId);
        captured!.AsOf.Should().Be(asOf);
        captured!.Top.Should().Be(5);
    }

    [Fact]
    public async Task GetSummary_DefaultsToTopThreeWhenAbsent()
    {
        GetRelationsSummaryRequest? captured = null;
        _mockSender
            .Setup(s => s.Send(It.IsAny<GetRelationsSummaryRequest>(), It.IsAny<CancellationToken>()))
            .Callback<object, CancellationToken>((r, _) => captured = (GetRelationsSummaryRequest)r)
            .ReturnsAsync(new GetRelationsSummaryResponse());

        await _controller.GetSummary(asOf: null, top: null);

        captured!.AsOf.Should().BeNull();
        captured!.Top.Should().Be(3);
    }
}
