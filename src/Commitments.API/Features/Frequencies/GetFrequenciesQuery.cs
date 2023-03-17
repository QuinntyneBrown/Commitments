using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.Frequencies;

 public class GetFrequenciesQueryRequest : IRequest<GetFrequenciesQueryResponse> { }

 public class GetFrequenciesQueryResponse
 {
     public IEnumerable<FrequencyDto> Frequencies { get; set; }
 }

 public class GetFrequenciesQueryHandler : IRequestHandler<GetFrequenciesQueryRequest, GetFrequenciesQueryResponse>
 {
     public IAppDbContext _context { get; set; }

     public GetFrequenciesQueryHandler(IAppDbContext context) => _context = context;

     public async Task<GetFrequenciesQueryResponse> Handle(GetFrequenciesQueryRequest request, CancellationToken cancellationToken)
         => new GetFrequenciesQueryResponse()
         {
             Frequencies = await _context.Frequencies
             .Include(x =>x.FrequencyType)
             .Select(x => FrequencyDto.FromFrequency(x))
             .ToListAsync()
         };
 }
