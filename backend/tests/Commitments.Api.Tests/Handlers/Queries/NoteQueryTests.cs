using Commitments.Data;
using Commitments.Domain.NoteAggregate;
using Commitments.Features.Note;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Queries;

public class NoteQueryTests
{
    private static CommitmentsDbContext NewCtx() =>
        new(new DbContextOptionsBuilder<CommitmentsDbContext>()
            .UseInMemoryDatabase($"note-q-{Guid.NewGuid()}").Options);

    [Fact]
    public async Task GetNotes_ReturnsCallerProfileOrderedByLastModifiedDesc(/* design 12 Slice B */)
    {
        var caller = Guid.NewGuid();
        var other = Guid.NewGuid();

        await using var ctx = NewCtx();
        var newer = Guid.NewGuid();
        var older = Guid.NewGuid();
        var foreign = Guid.NewGuid();
        ctx.Notes.Add(new Note { NoteId = older, ProfileId = caller, Title = "old", Slug = "old", Body = "", LastModifiedOn = new DateTime(2026, 1, 1) });
        ctx.Notes.Add(new Note { NoteId = newer, ProfileId = caller, Title = "new", Slug = "new", Body = "", LastModifiedOn = new DateTime(2026, 6, 1) });
        ctx.Notes.Add(new Note { NoteId = foreign, ProfileId = other, Title = "x", Slug = "x", Body = "" });
        await ctx.SaveChangesAsync(CancellationToken.None);

        var handler = new GetNotesByCurrentUserQueryHandler(ctx);
        var response = await handler.Handle(
            new GetNotesByCurrentUserRequest { ProfileId = caller },
            CancellationToken.None);

        response.Notes.Select(n => n.NoteId).Should().Equal(newer, older);
    }

    [Fact]
    public async Task GetNoteBySlug_ReturnsMatchInProfile()
    {
        var caller = Guid.NewGuid();
        await using var ctx = NewCtx();
        ctx.Notes.Add(new Note { NoteId = Guid.NewGuid(), ProfileId = caller, Title = "T", Slug = "the-slug", Body = "x" });
        await ctx.SaveChangesAsync(CancellationToken.None);

        var handler = new GetNoteBySlugQueryHandler(ctx);
        var response = await handler.Handle(
            new GetNoteBySlugRequest { ProfileId = caller, Slug = "the-slug" },
            CancellationToken.None);

        response.Note.Should().NotBeNull();
        response.Note!.Slug.Should().Be("the-slug");
    }
}
