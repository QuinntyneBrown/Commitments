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
     public IEnumerable<DigitalAssetApiModel> DigitalAssets { get; set; }
 }

 public class GetDigitalAssetsQueryHandler : IRequestHandler<GetDigitalAssetsQueryRequest, GetDigitalAssetsQueryResponse>
 {
     public IAppDbContext _context { get; set; }

     public GetDigitalAssetsQueryHandler(IAppDbContext context) => _context = context;

     public async Task<GetDigitalAssetsQueryResponse> Handle(GetDigitalAssetsQueryRequest request, CancellationToken cancellationToken)
         => new GetDigitalAssetsQueryResponse()
         {
             DigitalAssets = await _context.DigitalAssets.Select(x => DigitalAssetApiModel.FromDigitalAsset(x)).ToListAsync()
         };
 }
