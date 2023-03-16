using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.Behaviours;

 public class GetBehavioursQueryRequest : IRequest<GetBehavioursQueryResponse> { }

 public class GetBehavioursQueryResponse
 {
     public IEnumerable<BehaviourApiModel> Behaviours { get; set; }
 }

 public class GetBehavioursQueryHandler : IRequestHandler<GetBehavioursQueryRequest, GetBehavioursQueryResponse>
 {
     public IAppDbContext _context { get; set; }

     public GetBehavioursQueryHandler(IAppDbContext context) => _context = context;

     public async Task<GetBehavioursQueryResponse> Handle(GetBehavioursQueryRequest request, CancellationToken cancellationToken)
         => new GetBehavioursQueryResponse()
         {
             Behaviours = await _context.Behaviours.Include(x => x.BehaviourType).Select(x => BehaviourApiModel.FromBehaviour(x)).ToListAsync()
         };
 }
