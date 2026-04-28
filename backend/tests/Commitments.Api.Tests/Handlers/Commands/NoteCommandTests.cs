using Commitments.Data;
using Commitments.Domain.NoteAggregate;
using Commitments.Features.Note;
using Commitments.Testing.Common;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using NoteEntity = Commitments.Domain.NoteAggregate.Note;

namespace Commitments.Api.Tests.Handlers.Commands;

public class NoteCommandTests
{
    private static CommitmentsDbContext NewCtx() =>
        new(new DbContextOptionsBuilder<CommitmentsDbContext>()
            .UseInMemoryDatabase($"note-{Guid.NewGuid()}").Options);

    [Fact]
    public async Task SaveNote_StripsScriptTagsFromBody(/* design 12 Slice C / L2-041 AC#1 */)
    {
        await using var ctx = NewCtx();
        var handler = new SaveNoteCommandHandler(ctx, new NullEventBus());

        await handler.Handle(
            new SaveNoteRequest
            {
                Note = new NoteDto
                {
                    Title = "T",
                    Body = "<script>alert(1)</script><p>safe</p>",
                    ProfileId = Guid.NewGuid()
                }
            },
            CancellationToken.None);

        var saved = await ctx.Notes.SingleAsync();
        saved.Body.Should().NotContain("<script>");
        saved.Body.Should().Contain("<p>safe</p>");
    }

    [Fact]
    public async Task SaveNote_StripsOnEventHandlerAttributes()
    {
        await using var ctx = NewCtx();
        var handler = new SaveNoteCommandHandler(ctx, new NullEventBus());

        await handler.Handle(
            new SaveNoteRequest
            {
                Note = new NoteDto
                {
                    Title = "T",
                    Body = "<a href='x' onclick=\"alert(1)\">click</a>",
                    ProfileId = Guid.NewGuid()
                }
            },
            CancellationToken.None);

        var saved = await ctx.Notes.SingleAsync();
        saved.Body.Should().NotContain("onclick");
        saved.Body.Should().Contain("click");
    }

    [Fact]
    public async Task SaveNote_GeneratesSlugFromTitle()
    {
        await using var ctx = NewCtx();
        var handler = new SaveNoteCommandHandler(ctx, new NullEventBus());

        await handler.Handle(
            new SaveNoteRequest { Note = new NoteDto { Title = "Hello World!", Body = "x" } },
            CancellationToken.None);

        (await ctx.Notes.SingleAsync()).Slug.Should().Be("hello-world");
    }

    [Fact]
    public async Task SaveNote_DeduplicatesSlugWithinSameProfile()
    {
        await using var ctx = NewCtx();
        var profileId = Guid.NewGuid();
        ctx.Notes.Add(new NoteEntity { NoteId = Guid.NewGuid(), ProfileId = profileId, Title = "Hello", Slug = "hello", Body = "" });
        await ctx.SaveChangesAsync(CancellationToken.None);

        var handler = new SaveNoteCommandHandler(ctx, new NullEventBus());
        await handler.Handle(
            new SaveNoteRequest { Note = new NoteDto { Title = "Hello", ProfileId = profileId, Body = "x" } },
            CancellationToken.None);

        var slugs = await ctx.Notes.OrderBy(n => n.CreatedOn).Select(n => n.Slug).ToListAsync();
        slugs.Should().Equal("hello", "hello-2");
    }

    [Fact]
    public async Task RemoveNote_SoftDeletes(/* design 12 Slice D */)
    {
        await using var ctx = NewCtx();
        var id = Guid.NewGuid();
        ctx.Notes.Add(new NoteEntity { NoteId = id, Title = "x", Slug = "x", Body = "" });
        await ctx.SaveChangesAsync(CancellationToken.None);

        var handler = new RemoveNoteCommandHandler(ctx, new NullEventBus());
        await handler.Handle(new RemoveNoteRequest { NoteId = id }, CancellationToken.None);

        (await ctx.Notes.IgnoreQueryFilters().SingleAsync()).IsDeleted.Should().BeTrue();
        (await ctx.Notes.AnyAsync()).Should().BeFalse();
    }
}
