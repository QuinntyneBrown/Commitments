using Commitments.Testing.Common;
using Dashboard.Features.CardLayout;
using Dashboard.Data;
using FluentAssertions;
using Moq;
using Xunit;
using CardLayoutModel = Dashboard.Domain.CardLayoutAggregate.CardLayout;

namespace Dashboard.Api.Tests.Handlers.Commands;

public class UpdateCardLayoutHandlerTests
{
    [Fact]
    public async Task Handle_WhenCardLayoutExists_ShouldUpdateAndSave()
    {
        // Arrange
        var cardLayoutId = Guid.NewGuid();
        var cardLayout = new CardLayoutModel("Original", "Original Desc") { CardLayoutId = cardLayoutId };
        var cardLayouts = new List<CardLayoutModel> { cardLayout };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(cardLayouts);
        mockDbSet.Setup(d => d.FindAsync(cardLayoutId)).ReturnsAsync(cardLayout);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.CardLayouts).Returns(mockDbSet.Object);

        var handler = new UpdateCardLayoutRequestHandler(mockContext.Object);
        var request = new UpdateCardLayoutRequest
        {
            CardLayout = new CardLayoutDto { CardLayoutId = cardLayoutId, Name = "Updated Layout", Description = "Updated Desc" }
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.CardLayout.Should().NotBeNull();
        result.CardLayout.Name.Should().Be("Updated Layout");
        result.CardLayout.Description.Should().Be("Updated Desc");
        cardLayout.Name.Should().Be("Updated Layout");
        cardLayout.Description.Should().Be("Updated Desc");
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenCardLayoutNotFound_ShouldReturnEmptyResponse()
    {
        // Arrange
        var mockDbSet = MockDbSetFactory.CreateMockDbSet<CardLayoutModel>();
        mockDbSet.Setup(d => d.FindAsync(It.IsAny<Guid>())).ReturnsAsync((CardLayoutModel?)null);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.CardLayouts).Returns(mockDbSet.Object);

        var handler = new UpdateCardLayoutRequestHandler(mockContext.Object);
        var request = new UpdateCardLayoutRequest
        {
            CardLayout = new CardLayoutDto { CardLayoutId = Guid.NewGuid(), Name = "Updated", Description = "Desc" }
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.CardLayout.Should().BeNull();
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
