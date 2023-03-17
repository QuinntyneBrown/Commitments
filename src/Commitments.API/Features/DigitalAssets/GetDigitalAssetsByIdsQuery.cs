using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;


namespace Commitments.Api.Features.DigitalAssets;

 public class GetDigitalAssetsByIdsQueryRequest : IRequest<GetDigitalAssetsByIdsQueryResponse> {
     public Guid[] DigitalAssetIds { get; set; }
 }

 public class GetDigitalAssetsByIdsQueryResponse
 {
     public IEnumerable<DigitalAssetDto> DigitalAssets { get; set; }
 }

 public class GetDigitalAssetsByIdsQueryHandler : IRequestHandler<GetDigitalAssetsByIdsQueryRequest, GetDigitalAssetsByIdsQueryResponse>
 {
     public ICommimentsDbContext _context { get; set; }
     public GetDigitalAssetsByIdsQueryHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetDigitalAssetsByIdsQueryResponse> Handle(GetDigitalAssetsByIdsQueryRequest request, CancellationToken cancellationToken)
         => new GetDigitalAssetsByIdsQueryResponse()
         {
             DigitalAssets = await _context.DigitalAssets
             .Where(x => request.DigitalAssetIds.Contains(x.DigitalAssetId))
             .Select(x => DigitalAssetDto.FromDigitalAsset(x)).ToListAsync()
         };
 }
