using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.Frequencies;

 public class GetFrequenciesRequest : IRequest<GetFrequenciesResponse> { }

 public class GetFrequenciesResponse
 {
     public IEnumerable<FrequencyDto> Frequencies { get; set; }
 }

 public class GetFrequenciesQueryHandler : IRequestHandler<GetFrequenciesRequest, GetFrequenciesResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public GetFrequenciesQueryHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetFrequenciesResponse> Handle(GetFrequenciesRequest request, CancellationToken cancellationToken)
         => new GetFrequenciesResponse()
         {
             Frequencies = await _context.Frequencies
             .Include(x =>x.FrequencyType)
             .Select(x => FrequencyDto.FromFrequency(x))
             .ToListAsync()
         };
 }
