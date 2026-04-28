using Commitments.Data;
using Commitments.Domain.NoteAggregate;
using Commitments.Features.Note;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Commands;

public class NoteTagJoinTests
{
    private static CommitmentsDbContext NewCtx() =>
        new(new DbContextOptionsBuilder<CommitmentsDbContext>()
            .UseInMemoryDatabase($"notetag-{Guid.NewGuid()}").Options);

    [Fact]
    public async Task AddTagToNote_InsertsRow(/* design 15 Slice A */)
    {
        await using var ctx = NewCtx();
        var noteId = Guid.NewGuid();
        var tagId = Guid.NewGuid();

        var handler = new AddTagToNoteCommandHandler(ctx);
        await handler.Handle(new AddTagToNoteRequest { NoteId = noteId, TagId = tagId }, CancellationToken.None);

        var rows = await ctx.NoteTags.ToListAsync();
        rows.Should().ContainSingle(r => r.NoteId == noteId && r.TagId == tagId);
    }

    [Fact]
    public async Task AddTagToNote_IsIdempotent(/* design 15 Slice A */)
    {
        await using var ctx = NewCtx();
        var noteId = Guid.NewGuid();
        var tagId = Guid.NewGuid();

        var handler = new AddTagToNoteCommandHandler(ctx);
        await handler.Handle(new AddTagToNoteRequest { NoteId = noteId, TagId = tagId }, CancellationToken.None);
        await handler.Handle(new AddTagToNoteRequest { NoteId = noteId, TagId = tagId }, CancellationToken.None);

        (await ctx.NoteTags.CountAsync(r => r.NoteId == noteId && r.TagId == tagId)).Should().Be(1);
    }

    [Fact]
    public async Task RemoveTagFromNote_DeletesJoinRow(/* design 15 Slice D */)
    {
        await using var ctx = NewCtx();
        var noteId = Guid.NewGuid();
        var tagId = Guid.NewGuid();
        ctx.NoteTags.Add(new NoteTag { NoteId = noteId, TagId = tagId });
        await ctx.SaveChangesAsync(CancellationToken.None);

        var handler = new RemoveTagFromNoteCommandHandler(ctx);
        await handler.Handle(new RemoveTagFromNoteRequest { NoteId = noteId, TagId = tagId }, CancellationToken.None);

        (await ctx.NoteTags.AnyAsync()).Should().BeFalse();
    }
}
