using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.DigitalAssets;

 public class GetDigitalAssetsQueryRequest : IRequest<GetDigitalAssetsQueryResponse> { }

 public class GetDigitalAssetsQueryResponse
 {
     public IEnumerable<DigitalAssetDto> DigitalAssets { get; set; }
 }

 public class GetDigitalAssetsQueryHandler : IRequestHandler<GetDigitalAssetsQueryRequest, GetDigitalAssetsQueryResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public GetDigitalAssetsQueryHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetDigitalAssetsQueryResponse> Handle(GetDigitalAssetsQueryRequest request, CancellationToken cancellationToken)
         => new GetDigitalAssetsQueryResponse()
         {
             DigitalAssets = await _context.DigitalAssets.Select(x => DigitalAssetDto.FromDigitalAsset(x)).ToListAsync()
         };
 }
