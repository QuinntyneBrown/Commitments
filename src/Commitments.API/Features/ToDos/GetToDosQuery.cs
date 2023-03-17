using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.ToDos;

 public class GetToDosQueryRequest : IRequest<GetToDosQueryResponse> {
     public int ProfileId { get; set; }
 }

 public class GetToDosQueryResponse
 {
     public IEnumerable<ToDoDto> ToDos { get; set; }
 }

 public class GetToDosQueryHandler : IRequestHandler<GetToDosQueryRequest, GetToDosQueryResponse>
 {
     public IAppDbContext _context { get; set; }


     public async Task<GetToDosQueryResponse> Handle(GetToDosQueryRequest request, CancellationToken cancellationToken)
         => new GetToDosQueryResponse()
         {
             ToDos = await _context.ToDos
             .Where(x =>x.ProfileId == request.ProfileId)
             .Select(x => ToDoDto.FromToDo(x)).ToListAsync()
         };
 }
