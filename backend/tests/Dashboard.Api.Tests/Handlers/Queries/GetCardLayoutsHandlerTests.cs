using Commitments.Testing.Common;
using Dashboard.Features.CardLayout;
using Dashboard.Data;
using FluentAssertions;
using Moq;
using Xunit;
using CardLayoutModel = Dashboard.Domain.CardLayoutAggregate.CardLayout;

namespace Dashboard.Api.Tests.Handlers.Queries;

public class GetCardLayoutsHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnAllCardLayouts()
    {
        // Arrange
        var cardLayouts = new List<CardLayoutModel>
        {
            new CardLayoutModel("Layout 1", "Desc 1") { CardLayoutId = Guid.NewGuid() },
            new CardLayoutModel("Layout 2", "Desc 2") { CardLayoutId = Guid.NewGuid() }
        };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(cardLayouts);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.CardLayouts).Returns(mockDbSet.Object);

        var handler = new GetCardLayoutsRequestHandler(mockContext.Object);
        var request = new GetCardLayoutsRequest();

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.CardLayouts.Should().HaveCount(2);
        result.CardLayouts[0].Name.Should().Be("Layout 1");
        result.CardLayouts[1].Name.Should().Be("Layout 2");
    }

    [Fact]
    public async Task Handle_WhenNoCardLayouts_ShouldReturnEmptyList()
    {
        // Arrange
        var cardLayouts = new List<CardLayoutModel>();
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(cardLayouts);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.CardLayouts).Returns(mockDbSet.Object);

        var handler = new GetCardLayoutsRequestHandler(mockContext.Object);
        var request = new GetCardLayoutsRequest();

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.CardLayouts.Should().BeEmpty();
    }
}
