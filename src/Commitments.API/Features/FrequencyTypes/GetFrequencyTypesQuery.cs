using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.FrequencyTypes;

 public class GetFrequencyTypesQueryRequest : IRequest<GetFrequencyTypesQueryResponse> { }

 public class GetFrequencyTypesQueryResponse
 {
     public IEnumerable<FrequencyTypeApiModel> FrequencyTypes { get; set; }
 }

 public class GetFrequencyTypesQueryHandler : IRequestHandler<GetFrequencyTypesQueryRequest, GetFrequencyTypesQueryResponse>
 {
     public IAppDbContext _context { get; set; }

     public GetFrequencyTypesQueryHandler(IAppDbContext context) => _context = context;

     public async Task<GetFrequencyTypesQueryResponse> Handle(GetFrequencyTypesQueryRequest request, CancellationToken cancellationToken)
         => new GetFrequencyTypesQueryResponse()
         {
             FrequencyTypes = await _context.FrequencyTypes.Select(x => FrequencyTypeApiModel.FromFrequencyType(x)).ToListAsync()
         };
 }
