// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Controllers;
using Commitments.Features.Frequency;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Controllers;

public class FrequencyControllerTests
{
    private readonly Mock<ISender> _mockSender;
    private readonly FrequencyController _controller;

    public FrequencyControllerTests()
    {
        _mockSender = new Mock<ISender>();
        _controller = new FrequencyController(_mockSender.Object);
    }

    [Fact]
    public async Task Save_ShouldReturnSaveFrequencyResponse()
    {
        // Arrange
        var request = new SaveFrequencyRequest
        {
            Frequency = new FrequencyDto
            {
                FrequencyId = Guid.NewGuid(),
                FrequencyTypeId = Guid.NewGuid()
            }
        };
        var expectedResponse = new SaveFrequencyResponse
        {
            FrequencyId = request.Frequency.FrequencyId
        };
        _mockSender.Setup(s => s.Send(It.IsAny<SaveFrequencyRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Save(request);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.FrequencyId.Should().Be(expectedResponse.FrequencyId);
    }

    [Fact]
    public async Task Remove_ShouldCallSender()
    {
        // Arrange
        var request = new RemoveFrequencyRequest
        {
            FrequencyId = Guid.NewGuid()
        };
        _mockSender.Setup(s => s.Send(It.IsAny<RemoveFrequencyRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _controller.Remove(request);

        // Assert
        _mockSender.Verify(s => s.Send(It.Is<RemoveFrequencyRequest>(r => r.FrequencyId == request.FrequencyId), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_ShouldReturnFrequency()
    {
        // Arrange
        var frequencyId = Guid.NewGuid();
        var request = new GetFrequencyByIdRequest { FrequencyId = frequencyId };
        var expectedResponse = new GetFrequencyByIdResponse
        {
            Frequency = new FrequencyDto { FrequencyId = frequencyId }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<GetFrequencyByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetById(request);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.Frequency.FrequencyId.Should().Be(frequencyId);
    }

    [Fact]
    public async Task Get_ShouldReturnAllFrequencies()
    {
        // Arrange
        var expectedResponse = new GetFrequenciesResponse
        {
            Frequencies = new List<FrequencyDto>
            {
                new FrequencyDto { FrequencyId = Guid.NewGuid() },
                new FrequencyDto { FrequencyId = Guid.NewGuid() }
            }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<GetFrequenciesRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Get();

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.Frequencies.Should().HaveCount(2);
    }
}
