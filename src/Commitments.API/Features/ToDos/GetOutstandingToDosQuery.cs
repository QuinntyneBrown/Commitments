using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.ToDos;

 public class GetOutstandingToDosQueryRequest : IRequest<GetOutstandingToDosQueryResponse> {
     public int ProfileId { get; set; }
 }

 public class GetOutstandingToDosQueryResponse
 {
     public IEnumerable<ToDoDto> ToDos { get; set; }
 }

 public class GetOutstandingToDosQueryHandler : IRequestHandler<GetOutstandingToDosQueryRequest, GetOutstandingToDosQueryResponse>
 {
     public IAppDbContext _context { get; set; }
     public GetOutstandingToDosQueryHandler(IAppDbContext context) => _context = context;

     public async Task<GetOutstandingToDosQueryResponse> Handle(GetOutstandingToDosQueryRequest request, CancellationToken cancellationToken)
         => new GetOutstandingToDosQueryResponse()
         {
             ToDos = await _context.ToDos
             .Where(x => x.CompletedOn == null && x.ProfileId == request.ProfileId)
             .Select(x => ToDoDto.FromToDo(x))
             .ToListAsync()
         };
 }
