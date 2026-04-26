// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Controllers;
using Commitments.Features.Behaviour;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Controllers;

public class BehaviourControllerTests
{
    private readonly Mock<ISender> _mockSender;
    private readonly BehaviourController _controller;

    public BehaviourControllerTests()
    {
        _mockSender = new Mock<ISender>();
        _controller = new BehaviourController(_mockSender.Object);
    }

    [Fact]
    public async Task Save_ShouldReturnSaveBehaviourResponse()
    {
        // Arrange
        var request = new SaveBehaviourRequest
        {
            Behaviour = new BehaviourDto
            {
                BehaviourId = Guid.NewGuid(),
                Name = "Test Behaviour",
                BehaviourTypeId = Guid.NewGuid()
            }
        };
        var expectedResponse = new SaveBehaviourResponse
        {
            BehaviourId = request.Behaviour.BehaviourId
        };
        _mockSender.Setup(s => s.Send(It.IsAny<SaveBehaviourRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Save(request);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.BehaviourId.Should().Be(expectedResponse.BehaviourId);
    }

    [Fact]
    public async Task Remove_ShouldCallSender()
    {
        // Arrange
        var request = new RemoveBehaviourRequest
        {
            BehaviourId = Guid.NewGuid()
        };
        _mockSender.Setup(s => s.Send(It.IsAny<RemoveBehaviourRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _controller.Remove(request);

        // Assert
        _mockSender.Verify(s => s.Send(It.Is<RemoveBehaviourRequest>(r => r.BehaviourId == request.BehaviourId), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_ShouldReturnBehaviour()
    {
        // Arrange
        var behaviourId = Guid.NewGuid();
        var request = new GetBehaviourByIdRequest { BehaviourId = behaviourId };
        var expectedResponse = new GetBehaviourByIdResponse
        {
            Behaviour = new BehaviourDto { BehaviourId = behaviourId, Name = "Test" }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<GetBehaviourByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetById(request);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.Behaviour.BehaviourId.Should().Be(behaviourId);
    }

    [Fact]
    public async Task Get_ShouldReturnAllBehaviours()
    {
        // Arrange
        var expectedResponse = new GetBehavioursResponse
        {
            Behaviours = new List<BehaviourDto>
            {
                new BehaviourDto { BehaviourId = Guid.NewGuid(), Name = "Behaviour 1" },
                new BehaviourDto { BehaviourId = Guid.NewGuid(), Name = "Behaviour 2" }
            }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<GetBehavioursRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Get();

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.Behaviours.Should().HaveCount(2);
    }
}
