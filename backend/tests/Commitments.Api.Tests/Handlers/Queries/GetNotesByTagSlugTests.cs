using Commitments.Data;
using Commitments.Domain.NoteAggregate;
using Commitments.Domain.TagAggregate;
using Commitments.Features.Note;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Queries;

public class GetNotesByTagSlugTests
{
    private static CommitmentsDbContext NewCtx() =>
        new(new DbContextOptionsBuilder<CommitmentsDbContext>()
            .UseInMemoryDatabase($"notes-by-tag-{Guid.NewGuid()}").Options);

    [Fact]
    public async Task Handle_ReturnsCallerNotesTaggedWithSlug(/* design 15 Slice B */)
    {
        var caller = Guid.NewGuid();
        var other = Guid.NewGuid();

        await using var ctx = NewCtx();

        var weeklyTagId = Guid.NewGuid();
        var foreignTagId = Guid.NewGuid();
        ctx.Tags.Add(new Tag { TagId = weeklyTagId, ProfileId = caller, Name = "Weekly", Slug = "weekly" });
        ctx.Tags.Add(new Tag { TagId = foreignTagId, ProfileId = other, Name = "Weekly", Slug = "weekly" });

        var noteA = Guid.NewGuid();
        var noteB = Guid.NewGuid();
        var foreignNote = Guid.NewGuid();
        ctx.Notes.Add(new Note { NoteId = noteA, ProfileId = caller, Title = "A", Slug = "a", Body = "" });
        ctx.Notes.Add(new Note { NoteId = noteB, ProfileId = caller, Title = "B", Slug = "b", Body = "" });
        ctx.Notes.Add(new Note { NoteId = foreignNote, ProfileId = other, Title = "F", Slug = "f", Body = "" });

        ctx.NoteTags.Add(new NoteTag { NoteId = noteA, TagId = weeklyTagId });
        ctx.NoteTags.Add(new NoteTag { NoteId = noteB, TagId = weeklyTagId });
        ctx.NoteTags.Add(new NoteTag { NoteId = foreignNote, TagId = foreignTagId });
        await ctx.SaveChangesAsync(CancellationToken.None);

        var handler = new GetNotesByTagSlugQueryHandler(ctx);
        var response = await handler.Handle(
            new GetNotesByTagSlugRequest { ProfileId = caller, Slug = "weekly" },
            CancellationToken.None);

        response.Notes.Select(n => n.NoteId).Should().BeEquivalentTo(new[] { noteA, noteB });
    }

    [Fact]
    public async Task Handle_ReturnsEmpty_WhenSlugDoesNotMatchInProfile()
    {
        await using var ctx = NewCtx();
        var caller = Guid.NewGuid();
        ctx.Tags.Add(new Tag { TagId = Guid.NewGuid(), ProfileId = caller, Name = "X", Slug = "x" });
        await ctx.SaveChangesAsync(CancellationToken.None);

        var handler = new GetNotesByTagSlugQueryHandler(ctx);
        var response = await handler.Handle(
            new GetNotesByTagSlugRequest { ProfileId = caller, Slug = "missing" },
            CancellationToken.None);

        response.Notes.Should().BeEmpty();
    }
}
