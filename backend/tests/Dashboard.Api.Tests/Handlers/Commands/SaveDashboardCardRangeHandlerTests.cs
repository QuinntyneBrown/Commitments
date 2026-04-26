using Commitments.Testing.Common;
using Dashboard.Features.DashboardCard;
using Dashboard.Data;
using Dashboard.Domain.DashboardCardAggregate;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Dashboard.Api.Tests.Handlers.Commands;

public class SaveDashboardCardRangeHandlerTests
{
    [Fact]
    public async Task Handle_ShouldRemoveExistingAndAddNewCards()
    {
        // Arrange
        var dashboardId = Guid.NewGuid();
        var existingCard = new DashboardCard
        {
            DashboardCardId = Guid.NewGuid(),
            DashboardId = dashboardId,
            CardId = Guid.NewGuid(),
            CardLayoutId = Guid.NewGuid()
        };
        var existingCards = new List<DashboardCard> { existingCard };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(existingCards);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.DashboardCards).Returns(mockDbSet.Object);

        var handler = new SaveDashboardCardRangeRequestHandler(mockContext.Object);
        var newCardId1 = Guid.NewGuid();
        var newCardId2 = Guid.NewGuid();
        var newCardLayoutId1 = Guid.NewGuid();
        var newCardLayoutId2 = Guid.NewGuid();
        var request = new SaveDashboardCardRangeRequest
        {
            DashboardId = dashboardId,
            DashboardCards = new List<DashboardCardDto>
            {
                new DashboardCardDto { CardId = newCardId1, CardLayoutId = newCardLayoutId1 },
                new DashboardCardDto { CardId = newCardId2, CardLayoutId = newCardLayoutId2 }
            }
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.DashboardCardIds.Should().HaveCount(2);
        mockDbSet.Verify(d => d.RemoveRange(It.IsAny<IEnumerable<DashboardCard>>()), Times.Once);
        mockDbSet.Verify(d => d.AddRange(It.IsAny<IEnumerable<DashboardCard>>()), Times.Once);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithEmptyExisting_ShouldAddNewCards()
    {
        // Arrange
        var dashboardId = Guid.NewGuid();
        var existingCards = new List<DashboardCard>();
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(existingCards);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.DashboardCards).Returns(mockDbSet.Object);

        var handler = new SaveDashboardCardRangeRequestHandler(mockContext.Object);
        var request = new SaveDashboardCardRangeRequest
        {
            DashboardId = dashboardId,
            DashboardCards = new List<DashboardCardDto>
            {
                new DashboardCardDto { CardId = Guid.NewGuid(), CardLayoutId = Guid.NewGuid() }
            }
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.DashboardCardIds.Should().HaveCount(1);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
