using FluentValidation;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;
using Commitments.Core.Extensions;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.Notes;

 public class SaveNoteCommandValidator: AbstractValidator<SaveNoteCommandRequest> {
     public SaveNoteCommandValidator()
     {
         RuleFor(request => request.Note.NoteId).NotNull();
     }
 }

 public class SaveNoteCommandRequest : IRequest<SaveNoteCommandResponse> {
     public NoteApiModel Note { get; set; }
 }

 public class SaveNoteCommandResponse
 {            
     public int NoteId { get; set; }
 }

 public class SaveNoteCommandHandler : IRequestHandler<SaveNoteCommandRequest, SaveNoteCommandResponse>
 {
     private readonly IAppDbContext _context;

     public SaveNoteCommandHandler(IAppDbContext context) => _context = context;

     public async Task<SaveNoteCommandResponse> Handle(SaveNoteCommandRequest request, CancellationToken cancellationToken)
     {
         var note = await _context.Notes
             .Include(x => x.NoteTags)
             .Include("NoteTags.Tag")
             .SingleOrDefaultAsync(x => request.Note.NoteId == x.NoteId);

         if (note == null) _context.Notes.Add(note = new Note());

         note.Body = request.Note.Body;

         note.Title = request.Note.Title;

         note.Slug = request.Note.Title.GenerateSlug();

         note.NoteTags.Clear();

         foreach(var tag in request.Note.Tags)
         {
             note.NoteTags.Add(new NoteTag()
             {
                 Tag = (await _context.Tags.FindAsync(tag.TagId))
             });
         }

         await _context.SaveChangesAsync(cancellationToken);

         return new SaveNoteCommandResponse() { NoteId = note.NoteId };
     }
 }
