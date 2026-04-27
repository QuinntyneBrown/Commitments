// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Controllers;
using Commitments.Features.Commitment;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Controllers;

public class CommitmentControllerTests
{
    private readonly Mock<ISender> _mockSender;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly CommitmentController _controller;
    private readonly Guid _profileId = Guid.NewGuid();

    public CommitmentControllerTests()
    {
        _mockSender = new Mock<ISender>();
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["ProfileId"] = _profileId.ToString();
        _mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

        _controller = new CommitmentController(_mockHttpContextAccessor.Object, _mockSender.Object);
    }

    [Fact]
    public async Task Save_ShouldReturnSaveCommitmentResponse()
    {
        // Arrange
        var request = new SaveCommitmentRequest
        {
            Commitment = new CommitmentDto
            {
                CommitmentId = Guid.NewGuid(),
                BehaviourId = Guid.NewGuid()
            }
        };
        var expectedResponse = new SaveCommitmentResponse
        {
            CommitmentId = request.Commitment.CommitmentId
        };
        _mockSender.Setup(s => s.Send(It.IsAny<SaveCommitmentRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Save(request);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.CommitmentId.Should().Be(expectedResponse.CommitmentId);
    }

    [Fact]
    public async Task Save_ShouldSetProfileIdFromHttpContext()
    {
        // Arrange
        var request = new SaveCommitmentRequest
        {
            Commitment = new CommitmentDto
            {
                CommitmentId = Guid.NewGuid(),
                BehaviourId = Guid.NewGuid()
            }
        };
        var expectedResponse = new SaveCommitmentResponse
        {
            CommitmentId = request.Commitment.CommitmentId
        };
        _mockSender.Setup(s => s.Send(It.IsAny<SaveCommitmentRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        await _controller.Save(request);

        // Assert
        request.Commitment.ProfileId.Should().Be(_profileId);
    }

    [Fact]
    public async Task Remove_ShouldCallSender()
    {
        // Arrange
        var request = new RemoveCommitmentRequest
        {
            CommitmentId = Guid.NewGuid()
        };
        _mockSender.Setup(s => s.Send(It.IsAny<RemoveCommitmentRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _controller.Remove(request);

        // Assert
        _mockSender.Verify(s => s.Send(It.Is<RemoveCommitmentRequest>(r => r.CommitmentId == request.CommitmentId), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_ShouldReturnCommitment()
    {
        // Arrange
        var commitmentId = Guid.NewGuid();
        var request = new GetCommitmentByIdRequest { CommitmentId = commitmentId };
        var expectedResponse = new GetCommitmentByIdResponse
        {
            Commitment = new CommitmentDto { CommitmentId = commitmentId }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<GetCommitmentByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetById(request);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.Commitment.CommitmentId.Should().Be(commitmentId);
    }

    [Fact]
    public async Task Get_ShouldReturnAllCommitments()
    {
        // Arrange
        var expectedResponse = new GetCommitmentsResponse
        {
            Commitments = new List<CommitmentDto>
            {
                new CommitmentDto { CommitmentId = Guid.NewGuid() },
                new CommitmentDto { CommitmentId = Guid.NewGuid() }
            }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<GetCommitmentsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Get();

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.Commitments.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetPersonal_ShouldReturnPersonalCommitments()
    {
        // Arrange
        var expectedResponse = new GetPersonalCommitmentsResponse
        {
            Commitments = new List<CommitmentDto>
            {
                new CommitmentDto { CommitmentId = Guid.NewGuid(), ProfileId = _profileId }
            }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<GetPersonalCommitmentsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetPersonal();

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.Commitments.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetDaily_ShouldReturnDailyCommitments()
    {
        // Arrange
        var expectedResponse = new GetDailyCommitmentsResponse
        {
            Commitments = new List<CommitmentDto>
            {
                new CommitmentDto { CommitmentId = Guid.NewGuid() }
            }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<GetDailyCommitmentsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetDaily();

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.Commitments.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetDailyResults_ShouldDispatchWithProfileIdAndAsOf()
    {
        // Arrange
        var asOf = new DateTimeOffset(2026, 4, 27, 12, 0, 0, TimeSpan.Zero);
        var expected = new GetDailyResultsResponse
        {
            Mode = "review",
            AsOf = asOf,
            Date = "2026-04-27",
            Completed = 7,
            Total = 9
        };
        GetDailyResultsRequest? captured = null;
        _mockSender
            .Setup(s => s.Send(It.IsAny<GetDailyResultsRequest>(), It.IsAny<CancellationToken>()))
            .Callback<object, CancellationToken>((r, _) => captured = (GetDailyResultsRequest)r)
            .ReturnsAsync(expected);

        // Act
        var result = await _controller.GetDailyResults(asOf);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.Completed.Should().Be(7);
        result.Value!.Total.Should().Be(9);
        captured!.ProfileId.Should().Be(_profileId);
        captured!.AsOf.Should().Be(asOf);
    }

    [Fact]
    public async Task GetDailyResults_NoAsOf_DispatchesNullAsOf()
    {
        // Arrange
        var expected = new GetDailyResultsResponse { Mode = "live" };
        GetDailyResultsRequest? captured = null;
        _mockSender
            .Setup(s => s.Send(It.IsAny<GetDailyResultsRequest>(), It.IsAny<CancellationToken>()))
            .Callback<object, CancellationToken>((r, _) => captured = (GetDailyResultsRequest)r)
            .ReturnsAsync(expected);

        // Act
        await _controller.GetDailyResults(asOf: null);

        // Assert
        captured!.AsOf.Should().BeNull();
    }
}
