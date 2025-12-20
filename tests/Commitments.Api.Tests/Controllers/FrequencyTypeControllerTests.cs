// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Api.Controllers;
using Commitments.Core.AggregateModel.FrequencyTypeAggregate;
using Commitments.Core.AggregateModel.FrequencyTypeAggregate.Commands;
using Commitments.Core.AggregateModel.FrequencyTypeAggregate.Queries;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Controllers;

public class FrequencyTypeControllerTests
{
    private readonly Mock<ISender> _mockSender;
    private readonly FrequencyTypeController _controller;

    public FrequencyTypeControllerTests()
    {
        _mockSender = new Mock<ISender>();
        _controller = new FrequencyTypeController(_mockSender.Object);
    }

    [Fact]
    public async Task Save_ShouldReturnSaveFrequencyTypeResponse()
    {
        // Arrange
        var request = new SaveFrequencyTypeRequest
        {
            FrequencyType = new FrequencyTypeDto
            {
                FrequencyTypeId = Guid.NewGuid(),
                Name = "Daily"
            }
        };
        var expectedResponse = new SaveFrequencyTypeResponse
        {
            FrequencyTypeId = request.FrequencyType.FrequencyTypeId
        };
        _mockSender.Setup(s => s.Send(It.IsAny<SaveFrequencyTypeRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Save(request);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.FrequencyTypeId.Should().Be(expectedResponse.FrequencyTypeId);
    }

    [Fact]
    public async Task Remove_ShouldCallSender()
    {
        // Arrange
        var request = new RemoveFrequencyTypeRequest
        {
            FrequencyTypeId = Guid.NewGuid()
        };
        _mockSender.Setup(s => s.Send(It.IsAny<RemoveFrequencyTypeRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _controller.Remove(request);

        // Assert
        _mockSender.Verify(s => s.Send(It.Is<RemoveFrequencyTypeRequest>(r => r.FrequencyTypeId == request.FrequencyTypeId), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_ShouldReturnFrequencyType()
    {
        // Arrange
        var frequencyTypeId = Guid.NewGuid();
        var request = new GetFrequencyTypeByIdRequest { FrequencyTypeId = frequencyTypeId };
        var expectedResponse = new GetFrequencyTypeByIdResponse
        {
            FrequencyType = new FrequencyTypeDto { FrequencyTypeId = frequencyTypeId, Name = "Daily" }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<GetFrequencyTypeByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetById(request);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.FrequencyType.FrequencyTypeId.Should().Be(frequencyTypeId);
    }

    [Fact]
    public async Task Get_ShouldReturnAllFrequencyTypes()
    {
        // Arrange
        var expectedResponse = new GetFrequencyTypesResponse
        {
            FrequencyTypes = new List<FrequencyTypeDto>
            {
                new FrequencyTypeDto { FrequencyTypeId = Guid.NewGuid(), Name = "Daily" },
                new FrequencyTypeDto { FrequencyTypeId = Guid.NewGuid(), Name = "Weekly" }
            }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<GetFrequencyTypesRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Get();

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.FrequencyTypes.Should().HaveCount(2);
    }
}
