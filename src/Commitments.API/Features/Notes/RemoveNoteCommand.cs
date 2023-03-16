using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Api.Features.Notes;

 public class RemoveNoteCommandValidator : AbstractValidator<RemoveNoteCommandRequest>
 {
     public RemoveNoteCommandValidator()
     {
         RuleFor(request => request.NoteId).NotEqual(0);
     }
 }
 public class RemoveNoteCommandRequest : IRequest<RemoveNoteCommandResponse>
 {
     public int NoteId { get; set; }
 }

 public class RemoveNoteCommandResponse { }

 public class RemoveNoteCommandHandler : IRequestHandler<Request,Response>
 {
     private readonly IAppDbContext _context;

     public RemoveNoteCommandHandler(IAppDbContext context) => _context = context;

     public async Task<RemoveNoteCommandResponse> Handle(RemoveNoteCommandRequest request, CancellationToken cancellationToken)
     {
         _context.Notes.Remove(await _context.Notes.FindAsync(request.NoteId));
         await _context.SaveChangesAsync(cancellationToken);
         return new RemoveNoteCommandResponse() { };
     }
 }
