using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.Notes;

 public class GetNoteBySlugQueryRequest : IRequest<GetNoteBySlugQueryResponse> {
     public string Slug { get; set; }
 }

 public class GetNoteBySlugQueryResponse
 {
     public NoteApiModel Note { get; set; }
 }

 public class GetNoteBySlugQueryHandler : IRequestHandler<GetNoteBySlugQueryRequest, GetNoteBySlugQueryResponse>
 {
     private readonly IAppDbContext _context;

     public GetNoteBySlugQueryHandler(IAppDbContext context) => _context = context;

     public async Task<GetNoteBySlugQueryResponse> Handle(GetNoteBySlugQueryRequest request, CancellationToken cancellationToken)
     {
         return new GetNoteBySlugQueryResponse()
         {
             Note = NoteApiModel.FromNote(await _context.Notes
                 .Include(x => x.NoteTags)
                 .Include("NoteTags.Tag")
                 .Where(x => x.Slug == request.Slug)
                 .SingleAsync())
         };
     }
 }
