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
     public IEnumerable<BehaviourTypeDto> BehaviourTypes { get; set; }
 }

 public class GetBehaviourTypesQueryHandler : IRequestHandler<GetBehaviourTypesQueryRequest, GetBehaviourTypesQueryResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public GetBehaviourTypesQueryHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetBehaviourTypesQueryResponse> Handle(GetBehaviourTypesQueryRequest request, CancellationToken cancellationToken)
         => new GetBehaviourTypesQueryResponse()
         {
             BehaviourTypes = await _context.BehaviourTypes.Select(x => BehaviourTypeDto.FromBehaviourType(x)).ToListAsync()
         };
 }
