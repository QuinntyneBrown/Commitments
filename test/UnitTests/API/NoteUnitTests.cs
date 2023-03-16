using Commitments.Api.Features.Notes;
using Commitments.Api.Features.Tags;
using Commitments.Core.Entities;
using Commitments.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;


namespace UnitTests.API;

public class NoteUnitTests
{     
    [Fact]
    public async Task ShouldHandleSaveNoteCommandRequest()
    {

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "ShouldHandleSaveNoteCommandRequest")
            .Options;

        using (var context = new AppDbContext(options))
        {
            var handler = new SaveNoteCommand.Handler(context);

            context.Tags.Add(new Tag()
            {
                TagId = 1,
                Name = "Angular"
            });

            context.SaveChanges();

            var response = await handler.Handle(new SaveNoteCommand.Request()
            {
                Note = new NoteApiModel()
                {
                    Title = "Quinntyne",
                    Tags = new List<TagApiModel>() { new TagApiModel() { TagId = 1 } }
                }
            }, default(CancellationToken));

            Assert.Equal(1, response.NoteId);
        }
    }

    [Fact]
    public async Task ShouldHandleGetNoteByIdQueryRequest()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "ShouldHandleGetNoteByIdQueryRequest")
            .Options;

        using (var context = new AppDbContext(options))
        {
            context.Notes.Add(new Note()
            {
                NoteId = 1,
                Title = "Quinntyne",

            });

            context.SaveChanges();

            var handler = new GetNoteByIdQuery.Handler(context);

            var response = await handler.Handle(new GetNoteByIdQuery.Request()
            {
                NoteId = 1
            }, default(CancellationToken));

            Assert.Equal("Quinntyne", response.Note.Title);
        }
    }

    [Fact]
    public async Task ShouldHandleGetNoteBySlugQueryRequest()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "ShouldHandleGetNoteBySlugQueryRequest")
            .Options;

        using (var context = new AppDbContext(options))
        {
            context.Tags.Add(new Tag()
            {
                TagId = 1,
                Name = "Angular",
                Slug = "angular"
            });

            context.Notes.Add(new Note()
            {
                NoteId = 1,
                Title = "Quinntyne",
                Slug = "quinntyne",
                NoteTags = new List<NoteTag>() {
                    new NoteTag() { TagId = 1 }
                }
            });

            context.SaveChanges();

            var handler = new GetNoteBySlugQuery.Handler(context);

            var response = await handler.Handle(new GetNoteBySlugQuery.Request()
            {
                Slug = "quinntyne"
            }, default(CancellationToken));

            Assert.Equal("Quinntyne", response.Note.Title);
            Assert.Single(response.Note.Tags);
        }
    }

    [Fact]
    public async Task ShouldHandleGetNotesQueryRequest()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "ShouldHandleGetNotesQueryRequest")
            .Options;

        using (var context = new AppDbContext(options))
        {
            context.Notes.Add(new Commitments.Core.Entities.Note()
            {
                NoteId = 1,
                Title = "Quinntyne",

            });

            context.SaveChanges();

            var handler = new GetNotesQuery.Handler(context);

            var response = await handler.Handle(new GetNotesQuery.Request(), default(CancellationToken));

            Assert.Single(response.Notes);
        }
    }

    [Fact]
    public async Task ShouldHandleRemoveNoteCommandRequest()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "ShouldHandleRemoveNoteCommandRequest")
            .Options;

        using (var context = new AppDbContext(options))
        {
            context.Notes.Add(new Note()
            {
                NoteId = 1,
                Title = "Quinntyne",
            });

            context.SaveChanges();

            var handler = new RemoveNoteCommand.Handler(context);

            await handler.Handle(new RemoveNoteCommand.Request()
            {
                NoteId =  1 
            }, default(CancellationToken));

            Assert.Equal(0, context.Notes.Count());
        }
    }

    [Fact]
    public async Task ShouldHandleUpdateNoteCommandRequest()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "ShouldHandleUpdateNoteCommandRequest")
            .Options;

        using (var context = new AppDbContext(options))
        {
            context.Notes.Add(new Note()
            {
                NoteId = 1,
                Title = "Quinntyne"
            });

            context.SaveChanges();

            var handler = new SaveNoteCommand.Handler(context);

            var response = await handler.Handle(new SaveNoteCommand.Request()
            {
                Note = new NoteApiModel()
                {
                    NoteId = 1,
                    Title = "Quinntyne"
                }
            }, default(CancellationToken));

            Assert.Equal(1, response.NoteId);
            Assert.Equal("Quinntyne", context.Notes.Single(x => x.NoteId == 1).Title);
        }
    }
}
