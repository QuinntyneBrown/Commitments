// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Api.Controllers;
using Commitments.Core.AggregateModel.BehaviourTypeAggregate;
using Commitments.Core.AggregateModel.BehaviourTypeAggregate.Commands;
using Commitments.Core.AggregateModel.BehaviourTypeAggregate.Queries;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Controllers;

public class BehaviourTypeControllerTests
{
    private readonly Mock<ISender> _mockSender;
    private readonly BehaviourTypeController _controller;

    public BehaviourTypeControllerTests()
    {
        _mockSender = new Mock<ISender>();
        _controller = new BehaviourTypeController(_mockSender.Object);
    }

    [Fact]
    public async Task Save_ShouldReturnSaveBehaviourTypeResponse()
    {
        // Arrange
        var request = new SaveBehaviourTypeRequest
        {
            BehaviourType = new BehaviourTypeDto
            {
                BehaviourTypeId = Guid.NewGuid(),
                Name = "Test Behaviour Type"
            }
        };
        var expectedResponse = new SaveBehaviourTypeResponse
        {
            BehaviourTypeId = request.BehaviourType.BehaviourTypeId
        };
        _mockSender.Setup(s => s.Send(It.IsAny<SaveBehaviourTypeRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Save(request);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.BehaviourTypeId.Should().Be(expectedResponse.BehaviourTypeId);
    }

    [Fact]
    public async Task Remove_ShouldCallSender()
    {
        // Arrange
        var request = new RemoveBehaviourTypeRequest
        {
            BehaviourTypeId = Guid.NewGuid()
        };
        _mockSender.Setup(s => s.Send(It.IsAny<RemoveBehaviourTypeRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _controller.Remove(request);

        // Assert
        _mockSender.Verify(s => s.Send(It.Is<RemoveBehaviourTypeRequest>(r => r.BehaviourTypeId == request.BehaviourTypeId), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_ShouldReturnBehaviourType()
    {
        // Arrange
        var behaviourTypeId = Guid.NewGuid();
        var request = new GetBehaviourTypeByIdRequest { BehaviourTypeId = behaviourTypeId };
        var expectedResponse = new GetBehaviourTypeByIdResponse
        {
            BehaviourType = new BehaviourTypeDto { BehaviourTypeId = behaviourTypeId, Name = "Test" }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<GetBehaviourTypeByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetById(request);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.BehaviourType.BehaviourTypeId.Should().Be(behaviourTypeId);
    }

    [Fact]
    public async Task Get_ShouldReturnAllBehaviourTypes()
    {
        // Arrange
        var expectedResponse = new GetBehaviourTypesResponse
        {
            BehaviourTypes = new List<BehaviourTypeDto>
            {
                new BehaviourTypeDto { BehaviourTypeId = Guid.NewGuid(), Name = "Type 1" },
                new BehaviourTypeDto { BehaviourTypeId = Guid.NewGuid(), Name = "Type 2" }
            }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<GetBehaviourTypesRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Get();

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.BehaviourTypes.Should().HaveCount(2);
    }
}
