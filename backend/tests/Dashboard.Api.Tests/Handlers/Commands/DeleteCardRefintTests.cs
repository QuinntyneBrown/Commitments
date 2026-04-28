using Dashboard.Data;
using Dashboard.Domain.CardAggregate;
using Dashboard.Domain.DashboardCardAggregate;
using Dashboard.Features.Card;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Dashboard.Api.Tests.Handlers.Commands;

public class DeleteCardRefintTests
{
    private static DashboardDbContext NewCtx() =>
        new(new DbContextOptionsBuilder<DashboardDbContext>()
            .UseInMemoryDatabase($"card-refint-{Guid.NewGuid()}").Options);

    [Fact]
    public async Task Handle_WhenReferencedByDashboardCard_ThrowsBadHttpRequestException(/* design 16 Slice B */)
    {
        await using var ctx = NewCtx();
        var cardId = Guid.NewGuid();
        ctx.Cards.Add(new Card { CardId = cardId, Name = "x" });
        ctx.DashboardCards.Add(new DashboardCard { DashboardCardId = Guid.NewGuid(), CardId = cardId, DashboardId = Guid.NewGuid(), CardLayoutId = Guid.NewGuid() });
        await ctx.SaveChangesAsync(CancellationToken.None);

        var handler = new DeleteCardRequestHandler(ctx);

        var act = async () => await handler.Handle(new DeleteCardRequest { CardId = cardId }, CancellationToken.None);

        await act.Should().ThrowAsync<BadHttpRequestException>()
            .Where(e => e.StatusCode == 400 && e.Message.Contains("Cannot delete"));

        (await ctx.Cards.AnyAsync(c => c.CardId == cardId)).Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenNotReferenced_RemovesCard()
    {
        await using var ctx = NewCtx();
        var cardId = Guid.NewGuid();
        ctx.Cards.Add(new Card { CardId = cardId, Name = "x" });
        await ctx.SaveChangesAsync(CancellationToken.None);

        var handler = new DeleteCardRequestHandler(ctx);

        await handler.Handle(new DeleteCardRequest { CardId = cardId }, CancellationToken.None);

        (await ctx.Cards.IgnoreQueryFilters().AnyAsync(c => c.CardId == cardId && c.IsDeleted)).Should().BeTrue();
    }
}
