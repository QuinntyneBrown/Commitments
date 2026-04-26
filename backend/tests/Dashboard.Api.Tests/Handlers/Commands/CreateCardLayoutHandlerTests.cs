using Commitments.Testing.Common;
using Dashboard.Features.CardLayout;
using Dashboard.Data;
using FluentAssertions;
using Moq;
using Xunit;
using CardLayoutModel = Dashboard.Domain.CardLayoutAggregate.CardLayout;

namespace Dashboard.Api.Tests.Handlers.Commands;

public class CreateCardLayoutHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateCardLayoutAndSave()
    {
        // Arrange
        var cardLayouts = new List<CardLayoutModel>();
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(cardLayouts);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.CardLayouts).Returns(mockDbSet.Object);

        var handler = new CreateCardLayoutRequestHandler(mockContext.Object);
        var request = new CreateCardLayoutRequest
        {
            CardLayout = new CardLayoutDto { Name = "Test Layout", Description = "Test Desc" }
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.CardLayout.Should().NotBeNull();
        result.CardLayout.Name.Should().Be("Test Layout");
        result.CardLayout.Description.Should().Be("Test Desc");
        cardLayouts.Should().HaveCount(1);
        cardLayouts[0].Name.Should().Be("Test Layout");
        cardLayouts[0].Description.Should().Be("Test Desc");
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
