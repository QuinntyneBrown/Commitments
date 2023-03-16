using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.Activities;

 public class GetActivitiesQueryRequest : IRequest<GetActivitiesQueryResponse> { }

 public class GetActivitiesQueryResponse
 {
     public IEnumerable<ActivityApiModel> Activities { get; set; }
 }

 public class GetActivitiesQueryHandler : IRequestHandler<GetActivitiesQueryRequest, GetActivitiesQueryResponse>
 {
     public IAppDbContext _context { get; set; }

     public GetActivitiesQueryHandler(IAppDbContext context) => _context = context;

     public async Task<GetActivitiesQueryResponse> Handle(GetActivitiesQueryRequest request, CancellationToken cancellationToken)
         => new GetActivitiesQueryResponse()
         {
             Activities = await _context.Activities
             .Include(x => x.Behaviour)
             .Include("Behaviour.BehaviourType")
             .Select(x => ActivityApiModel.FromActivity(x)).ToListAsync()
         };
 }
