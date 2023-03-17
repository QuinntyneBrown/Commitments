// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;
using Commitments.Core.AggregateModel;
using Commitments.Core.Interfaces;
using Commitments.Core.Extensions;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.Notes;

 public class SaveNoteCommandValidator: AbstractValidator<SaveNoteRequest> {
     public SaveNoteCommandValidator()
     {
         RuleFor(request => request.Note.NoteId).NotNull();
     }
 }

 public class SaveNoteRequest : IRequest<SaveNoteResponse> {
     public NoteDto Note { get; set; }
 }

 public class SaveNoteResponse
 {            
     public int NoteId { get; set; }
 }

 public class SaveNoteCommandHandler : IRequestHandler<SaveNoteRequest, SaveNoteResponse>
 {
     private readonly ICommimentsDbContext _context;

     public SaveNoteCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task<SaveNoteResponse> Handle(SaveNoteRequest request, CancellationToken cancellationToken)
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

         return new SaveNoteResponse() { NoteId = note.NoteId };
     }
 }

