using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.Notes;

 public class GetNotesQueryRequest : IRequest<GetNotesQueryResponse> { }

 public class GetNotesQueryResponse
 {
     public IEnumerable<NoteDto> Notes { get; set; }
 }

 public class GetNotesQueryHandler : IRequestHandler<GetNotesQueryRequest, GetNotesQueryResponse>
 {
     private readonly IAppDbContext _context;

     public GetNotesQueryHandler(IAppDbContext context) => _context = context;

     public async Task<GetNotesQueryResponse> Handle(GetNotesQueryRequest request, CancellationToken cancellationToken)
         => new GetNotesQueryResponse()
         {
             Notes = await _context.Notes.Select(x => NoteDto.FromNote(x, true)).ToListAsync()
         };
 }
