using Commitments.Data;
using Commitments.Domain.TagAggregate;
using Commitments.Features.Tag;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Queries;

public class TagQueryTests
{
    private static CommitmentsDbContext NewCtx() =>
        new(new DbContextOptionsBuilder<CommitmentsDbContext>()
            .UseInMemoryDatabase($"tag-q-{Guid.NewGuid()}").Options);

    [Fact]
    public async Task GetTags_ReturnsCallerProfileOrderedByName(/* design 14 Slice B */)
    {
        var caller = Guid.NewGuid();
        var other = Guid.NewGuid();

        await using var ctx = NewCtx();
        ctx.Tags.Add(new Tag { TagId = Guid.NewGuid(), ProfileId = caller, Name = "Zeta", Slug = "zeta" });
        ctx.Tags.Add(new Tag { TagId = Guid.NewGuid(), ProfileId = caller, Name = "Alpha", Slug = "alpha" });
        ctx.Tags.Add(new Tag { TagId = Guid.NewGuid(), ProfileId = other, Name = "ignored", Slug = "ignored" });
        await ctx.SaveChangesAsync(CancellationToken.None);

        var handler = new GetTagsQueryHandler(ctx);
        var response = await handler.Handle(
            new GetTagsRequest { ProfileId = caller },
            CancellationToken.None);

        response.Tags.Select(t => t.Name).Should().Equal("Alpha", "Zeta");
    }

    [Fact]
    public async Task GetTagBySlug_ReturnsMatchInProfile()
    {
        var caller = Guid.NewGuid();
        await using var ctx = NewCtx();
        ctx.Tags.Add(new Tag { TagId = Guid.NewGuid(), ProfileId = caller, Name = "T", Slug = "the-slug" });
        await ctx.SaveChangesAsync(CancellationToken.None);

        var handler = new GetTagBySlugQueryHandler(ctx);
        var response = await handler.Handle(
            new GetTagBySlugRequest { ProfileId = caller, Slug = "the-slug" },
            CancellationToken.None);

        response.Tag.Should().NotBeNull();
        response.Tag!.Slug.Should().Be("the-slug");
    }
}
