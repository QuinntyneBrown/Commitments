// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Api.Controllers;
using Commitments.Core.Model.CardAggregate;
using Commitments.Core.Model.CardAggregate.Commands;
using Commitments.Core.Model.CardAggregate.Queries;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Controllers;

public class CardControllerTests
{
    private readonly Mock<ISender> _mockSender;
    private readonly CardController _controller;

    public CardControllerTests()
    {
        _mockSender = new Mock<ISender>();
        _controller = new CardController(_mockSender.Object);
    }

    [Fact]
    public async Task Create_ShouldReturnCreateCardResponse()
    {
        // Arrange
        var request = new CreateCardRequest
        {
            Card = new CardDto
            {
                CardId = Guid.NewGuid(),
                Name = "Test Card",
                Description = "Test Description"
            }
        };
        var expectedResponse = new CreateCardResponse
        {
            Card = request.Card
        };
        _mockSender.Setup(s => s.Send(It.IsAny<CreateCardRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Create(request);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.Card.CardId.Should().Be(request.Card.CardId);
    }

    [Fact]
    public async Task Update_ShouldReturnUpdateCardResponse()
    {
        // Arrange
        var request = new UpdateCardRequest
        {
            Card = new CardDto
            {
                CardId = Guid.NewGuid(),
                Name = "Updated Card"
            }
        };
        var expectedResponse = new UpdateCardResponse
        {
            Card = request.Card
        };
        _mockSender.Setup(s => s.Send(It.IsAny<UpdateCardRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Update(request);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.Card.Name.Should().Be("Updated Card");
    }

    [Fact]
    public async Task Delete_ShouldCallSender()
    {
        // Arrange
        var request = new DeleteCardRequest
        {
            CardId = Guid.NewGuid()
        };
        var expectedResponse = new DeleteCardResponse();
        _mockSender.Setup(s => s.Send(It.IsAny<DeleteCardRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        await _controller.Delete(request);

        // Assert
        _mockSender.Verify(s => s.Send(It.Is<DeleteCardRequest>(r => r.CardId == request.CardId), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_ShouldReturnCard()
    {
        // Arrange
        var cardId = Guid.NewGuid();
        var request = new GetCardByIdRequest { CardId = cardId };
        var expectedResponse = new GetCardByIdResponse
        {
            Card = new CardDto { CardId = cardId, Name = "Test Card" }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<GetCardByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetById(request);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.Card.CardId.Should().Be(cardId);
    }

    [Fact]
    public async Task Get_ShouldReturnAllCards()
    {
        // Arrange
        var expectedResponse = new GetCardsResponse
        {
            Cards = new List<CardDto>
            {
                new CardDto { CardId = Guid.NewGuid(), Name = "Card 1" },
                new CardDto { CardId = Guid.NewGuid(), Name = "Card 2" }
            }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<GetCardsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Get();

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.Cards.Should().HaveCount(2);
    }
}
