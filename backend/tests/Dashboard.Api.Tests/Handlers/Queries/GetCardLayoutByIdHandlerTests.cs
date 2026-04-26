using Commitments.Testing.Common;
using Dashboard.Features.CardLayout;
using Dashboard.Data;
using FluentAssertions;
using Moq;
using Xunit;
using CardLayoutModel = Dashboard.Domain.CardLayoutAggregate.CardLayout;

namespace Dashboard.Api.Tests.Handlers.Queries;

public class GetCardLayoutByIdHandlerTests
{
    [Fact]
    public async Task Handle_WhenCardLayoutExists_ShouldReturnCardLayout()
    {
        // Arrange
        var cardLayoutId = Guid.NewGuid();
        var cardLayout = new CardLayoutModel("Test Layout", "Test Desc") { CardLayoutId = cardLayoutId };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet<CardLayoutModel>();
        mockDbSet.Setup(d => d.FindAsync(cardLayoutId)).ReturnsAsync(cardLayout);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.CardLayouts).Returns(mockDbSet.Object);

        var handler = new GetCardLayoutByIdRequestHandler(mockContext.Object);
        var request = new GetCardLayoutByIdRequest { CardLayoutId = cardLayoutId };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.CardLayout.Should().NotBeNull();
        result.CardLayout!.CardLayoutId.Should().Be(cardLayoutId);
        result.CardLayout.Name.Should().Be("Test Layout");
        result.CardLayout.Description.Should().Be("Test Desc");
    }

    [Fact]
    public async Task Handle_WhenCardLayoutNotFound_ShouldReturnNullCardLayout()
    {
        // Arrange
        var mockDbSet = MockDbSetFactory.CreateMockDbSet<CardLayoutModel>();
        mockDbSet.Setup(d => d.FindAsync(It.IsAny<Guid>())).ReturnsAsync((CardLayoutModel?)null);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.CardLayouts).Returns(mockDbSet.Object);

        var handler = new GetCardLayoutByIdRequestHandler(mockContext.Object);
        var request = new GetCardLayoutByIdRequest { CardLayoutId = Guid.NewGuid() };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.CardLayout.Should().BeNull();
    }
}
