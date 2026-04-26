using Dashboard.Api.Controllers;
using Dashboard.Api.Features.Card;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dashboard.Api.Tests.Controllers;

public class CardControllerTests
{
    private readonly Mock<ISender> _mockSender;
    private readonly Mock<ILogger<CardController>> _mockLogger;
    private readonly CardController _controller;

    public CardControllerTests()
    {
        _mockSender = new Mock<ISender>();
        _mockLogger = new Mock<ILogger<CardController>>();
        _controller = new CardController(_mockSender.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Create_ShouldReturnCreateCardResponse()
    {
        // Arrange
        var request = new CreateCardRequest
        {
            Card = new CardDto { Name = "New Card", Description = "Desc" }
        };
        var expectedResponse = new CreateCardResponse
        {
            Card = new CardDto { CardId = Guid.NewGuid(), Name = "New Card", Description = "Desc" }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<CreateCardRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Create(request, CancellationToken.None);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.Card.Name.Should().Be("New Card");
    }

    [Fact]
    public async Task Update_ShouldReturnUpdateCardResponse()
    {
        // Arrange
        var cardId = Guid.NewGuid();
        var request = new UpdateCardRequest
        {
            Card = new CardDto { CardId = cardId, Name = "Updated", Description = "Updated Desc" }
        };
        var expectedResponse = new UpdateCardResponse
        {
            Card = new CardDto { CardId = cardId, Name = "Updated", Description = "Updated Desc" }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<UpdateCardRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Update(request, CancellationToken.None);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.Card.Name.Should().Be("Updated");
    }

    [Fact]
    public async Task Get_ShouldReturnCards()
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
        var result = await _controller.Get(CancellationToken.None);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.Cards.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetById_WhenFound_ShouldReturnCard()
    {
        // Arrange
        var cardId = Guid.NewGuid();
        var expectedResponse = new GetCardByIdResponse
        {
            Card = new CardDto { CardId = cardId, Name = "Found Card" }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<GetCardByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetById(cardId, CancellationToken.None);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.Card!.CardId.Should().Be(cardId);
    }

    [Fact]
    public async Task GetById_WhenNotFound_ShouldReturnNotFound()
    {
        // Arrange
        var cardId = Guid.NewGuid();
        var expectedResponse = new GetCardByIdResponse { Card = null };
        _mockSender.Setup(s => s.Send(It.IsAny<GetCardByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetById(cardId, CancellationToken.None);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Delete_ShouldReturnDeleteCardResponse()
    {
        // Arrange
        var cardId = Guid.NewGuid();
        var expectedResponse = new DeleteCardResponse();
        _mockSender.Setup(s => s.Send(It.IsAny<DeleteCardRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Delete(cardId, CancellationToken.None);

        // Assert
        result.Value.Should().NotBeNull();
        _mockSender.Verify(s => s.Send(It.Is<DeleteCardRequest>(r => r.CardId == cardId), It.IsAny<CancellationToken>()), Times.Once);
    }
}
