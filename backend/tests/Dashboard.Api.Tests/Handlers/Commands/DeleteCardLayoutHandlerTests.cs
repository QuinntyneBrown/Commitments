using Commitments.Testing.Common;
using Dashboard.Domain.DashboardCardAggregate;
using Dashboard.Features.CardLayout;
using Dashboard.Data;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;
using CardLayoutModel = Dashboard.Domain.CardLayoutAggregate.CardLayout;

namespace Dashboard.Api.Tests.Handlers.Commands;

public class DeleteCardLayoutHandlerTests
{
    [Fact]
    public async Task Handle_WhenCardLayoutExists_ShouldRemoveAndSave()
    {
        // Arrange
        var cardLayoutId = Guid.NewGuid();
        var cardLayout = new CardLayoutModel("ToDelete", "Desc") { CardLayoutId = cardLayoutId };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet<CardLayoutModel>();
        mockDbSet.Setup(d => d.FindAsync(cardLayoutId)).ReturnsAsync(cardLayout);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.CardLayouts).Returns(mockDbSet.Object);

        var handler = new DeleteCardLayoutRequestHandler(mockContext.Object);
        var request = new DeleteCardLayoutRequest { CardLayoutId = cardLayoutId };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        mockDbSet.Verify(d => d.Remove(cardLayout), Times.Once);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenCardLayoutNotFound_ShouldNotCallSave()
    {
        // Arrange
        var mockDbSet = MockDbSetFactory.CreateMockDbSet<CardLayoutModel>();
        mockDbSet.Setup(d => d.FindAsync(It.IsAny<Guid>())).ReturnsAsync((CardLayoutModel?)null);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.CardLayouts).Returns(mockDbSet.Object);

        var handler = new DeleteCardLayoutRequestHandler(mockContext.Object);
        var request = new DeleteCardLayoutRequest { CardLayoutId = Guid.NewGuid() };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        mockDbSet.Verify(d => d.Remove(It.IsAny<CardLayoutModel>()), Times.Never);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenReferencedByDashboardCard_ThrowsBadHttpRequestException(/* design 17 Slice B */)
    {
        var cardLayoutId = Guid.NewGuid();
        var existing = new CardLayoutModel("Layout", "x") { CardLayoutId = cardLayoutId };
        var referencingTile = new DashboardCard { DashboardCardId = Guid.NewGuid(), CardLayoutId = cardLayoutId, CardId = Guid.NewGuid(), DashboardId = Guid.NewGuid() };

        var layoutSet = MockDbSetFactory.CreateMockDbSet(new List<CardLayoutModel> { existing });
        layoutSet.Setup(d => d.FindAsync(cardLayoutId)).ReturnsAsync(existing);

        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.CardLayouts).Returns(layoutSet.Object);
        mockContext.Setup(c => c.DashboardCards).Returns(MockDbSetFactory.CreateMockDbSet(new List<DashboardCard> { referencingTile }).Object);

        var handler = new DeleteCardLayoutRequestHandler(mockContext.Object);

        var act = async () => await handler.Handle(new DeleteCardLayoutRequest { CardLayoutId = cardLayoutId }, CancellationToken.None);

        await act.Should().ThrowAsync<BadHttpRequestException>()
            .Where(e => e.StatusCode == 400 && e.Message.Contains("Cannot delete"));

        layoutSet.Verify(d => d.Remove(It.IsAny<CardLayoutModel>()), Times.Never);
    }
}
