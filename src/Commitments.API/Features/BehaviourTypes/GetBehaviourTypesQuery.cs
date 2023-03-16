using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.BehaviourTypes;

 public class GetBehaviourTypesQueryRequest : IRequest<GetBehaviourTypesQueryResponse> { }

 public class GetBehaviourTypesQueryResponse
 {
     public IEnumerable<BehaviourTypeApiModel> BehaviourTypes { get; set; }
 }

 public class GetBehaviourTypesQueryHandler : IRequestHandler<GetBehaviourTypesQueryRequest, GetBehaviourTypesQueryResponse>
 {
     public IAppDbContext _context { get; set; }

     public GetBehaviourTypesQueryHandler(IAppDbContext context) => _context = context;

     public async Task<GetBehaviourTypesQueryResponse> Handle(GetBehaviourTypesQueryRequest request, CancellationToken cancellationToken)
         => new GetBehaviourTypesQueryResponse()
         {
             BehaviourTypes = await _context.BehaviourTypes.Select(x => BehaviourTypeApiModel.FromBehaviourType(x)).ToListAsync()
         };
 }
