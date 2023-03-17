using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.ToDos;

 public class GetToDosRequest : IRequest<GetToDosResponse> {
     public int ProfileId { get; set; }
 }

 public class GetToDosResponse
 {
     public IEnumerable<ToDoDto> ToDos { get; set; }
 }

 public class GetToDosQueryHandler : IRequestHandler<GetToDosRequest, GetToDosResponse>
 {
     public ICommimentsDbContext _context { get; set; }


     public async Task<GetToDosResponse> Handle(GetToDosRequest request, CancellationToken cancellationToken)
         => new GetToDosResponse()
         {
             ToDos = await _context.ToDos
             .Where(x =>x.ProfileId == request.ProfileId)
             .Select(x => ToDoDto.FromToDo(x)).ToListAsync()
         };
 }
