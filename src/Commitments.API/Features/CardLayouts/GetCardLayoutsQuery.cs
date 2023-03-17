using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.CardLayouts;

 public class GetCardLayoutsQueryRequest : IRequest<GetCardLayoutsQueryResponse> { }

 public class GetCardLayoutsQueryResponse
 {
     public IEnumerable<CardLayoutDto> CardLayouts { get; set; }
 }

 public class GetCardLayoutsQueryHandler : IRequestHandler<GetCardLayoutsQueryRequest, GetCardLayoutsQueryResponse>
 {
     public ICommimentsDbContext _context { get; set; }


     public async Task<GetCardLayoutsQueryResponse> Handle(GetCardLayoutsQueryRequest request, CancellationToken cancellationToken)
         => new GetCardLayoutsQueryResponse()
         {
             CardLayouts = await _context.CardLayouts.Select(x => CardLayoutDto.FromCardLayout(x)).ToListAsync()
         };
 }
