using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.Activities;

 public class GetActivitiesRequest : IRequest<GetActivitiesResponse> { }

 public class GetActivitiesResponse
 {
     public IEnumerable<ActivityDto> Activities { get; set; }
 }

 public class GetActivitiesQueryHandler : IRequestHandler<GetActivitiesRequest, GetActivitiesResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public GetActivitiesQueryHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetActivitiesResponse> Handle(GetActivitiesRequest request, CancellationToken cancellationToken)
         => new GetActivitiesResponse()
         {
             Activities = await _context.Activities
             .Include(x => x.Behaviour)
             .Include("Behaviour.BehaviourType")
             .Select(x => ActivityDto.FromActivity(x)).ToListAsync()
         };
 }
