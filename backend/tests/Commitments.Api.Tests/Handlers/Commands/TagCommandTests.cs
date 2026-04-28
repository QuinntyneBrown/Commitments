using Commitments.Data;
using Commitments.Domain.NoteAggregate;
using Commitments.Domain.TagAggregate;
using Commitments.Features.Tag;
using Commitments.Testing.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Commands;

public class TagCommandTests
{
    private static CommitmentsDbContext NewCtx() =>
        new(new DbContextOptionsBuilder<CommitmentsDbContext>()
            .UseInMemoryDatabase($"tag-{Guid.NewGuid()}").Options);

    [Fact]
    public async Task SaveTag_GeneratesSlugFromName(/* design 14 Slice C */)
    {
        await using var ctx = NewCtx();
        var handler = new SaveTagCommandHandler(ctx, new NullEventBus());

        await handler.Handle(
            new SaveTagRequest { Tag = new TagDto { Name = "Hello World!", ProfileId = Guid.NewGuid() } },
            CancellationToken.None);

        (await ctx.Tags.SingleAsync()).Slug.Should().Be("hello-world");
    }

    [Fact]
    public async Task SaveTag_DeduplicatesSlugWithinSameProfile()
    {
        await using var ctx = NewCtx();
        var profileId = Guid.NewGuid();
        ctx.Tags.Add(new Tag { TagId = Guid.NewGuid(), ProfileId = profileId, Name = "Weekly", Slug = "weekly" });
        await ctx.SaveChangesAsync(CancellationToken.None);

        var handler = new SaveTagCommandHandler(ctx, new NullEventBus());
        await handler.Handle(
            new SaveTagRequest { Tag = new TagDto { Name = "weekly", ProfileId = profileId } },
            CancellationToken.None);

        var slugs = await ctx.Tags.OrderBy(t => t.CreatedOn).Select(t => t.Slug).ToListAsync();
        slugs.Should().Equal("weekly", "weekly-2");
    }

    [Fact]
    public void SaveTagValidator_FailsOnEmptyName()
    {
        var validator = new SaveTagCommandValidator();
        var result = validator.Validate(new SaveTagRequest { Tag = new TagDto { Name = "" } });
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task RemoveTag_WhenReferencedByNoteTag_ThrowsBadHttpRequestException(/* design 14 Slice D */)
    {
        await using var ctx = NewCtx();
        var tagId = Guid.NewGuid();
        ctx.Tags.Add(new Tag { TagId = tagId, ProfileId = Guid.NewGuid(), Name = "T", Slug = "t" });
        ctx.NoteTags.Add(new NoteTag { NoteId = Guid.NewGuid(), TagId = tagId });
        await ctx.SaveChangesAsync(CancellationToken.None);

        var handler = new RemoveTagCommandHandler(ctx, new NullEventBus());

        var act = async () => await handler.Handle(
            new RemoveTagRequest { TagId = tagId },
            CancellationToken.None);

        await act.Should().ThrowAsync<BadHttpRequestException>()
            .Where(e => e.StatusCode == 400 && e.Message.Contains("Cannot delete"));

        (await ctx.Tags.SingleAsync()).IsDeleted.Should().BeFalse();
    }

    [Fact]
    public async Task RemoveTag_WhenNotReferenced_SoftDeletes()
    {
        await using var ctx = NewCtx();
        var tagId = Guid.NewGuid();
        ctx.Tags.Add(new Tag { TagId = tagId, ProfileId = Guid.NewGuid(), Name = "T", Slug = "t" });
        await ctx.SaveChangesAsync(CancellationToken.None);

        var handler = new RemoveTagCommandHandler(ctx, new NullEventBus());
        await handler.Handle(new RemoveTagRequest { TagId = tagId }, CancellationToken.None);

        (await ctx.Tags.IgnoreQueryFilters().SingleAsync()).IsDeleted.Should().BeTrue();
    }
}
