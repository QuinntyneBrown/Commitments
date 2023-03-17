using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Api.Features.Notes;

 public class GetNoteByIdQueryValidator : AbstractValidator<GetNoteByIdQueryRequest>
 {
     public GetNoteByIdQueryValidator()
     {
         RuleFor(request => request.NoteId).NotEqual(0);
     }
 }

 public class GetNoteByIdQueryRequest : IRequest<GetNoteByIdQueryResponse> {
     public int NoteId { get; set; }
 }

 public class GetNoteByIdQueryResponse
 {
     public NoteDto Note { get; set; }
 }

 public class GetNoteByIdQueryHandler : IRequestHandler<GetNoteByIdQueryRequest, GetNoteByIdQueryResponse>
 {
     private readonly ICommimentsDbContext _context;

     public GetNoteByIdQueryHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetNoteByIdQueryResponse> Handle(GetNoteByIdQueryRequest request, CancellationToken cancellationToken)
         => new GetNoteByIdQueryResponse()
         {
             Note = NoteDto.FromNote(await _context.Notes.FindAsync(request.NoteId))
         };            
 }
