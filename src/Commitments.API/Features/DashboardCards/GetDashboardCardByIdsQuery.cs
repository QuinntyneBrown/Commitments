using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.DashboardCards;

 public class GetDashboardCardByIdsQueryRequest : IRequest<GetDashboardCardByIdsQueryResponse> {
     public int[] DashboardCardIds { get; set; }
 }

 public class GetDashboardCardByIdsQueryResponse
 {
     public IEnumerable<DashboardCardApiModel> DashboardCards { get; set; }
 }

 public class GetDashboardCardByIdsQueryHandler : IRequestHandler<GetDashboardCardByIdsQueryRequest, GetDashboardCardByIdsQueryResponse>
 {
     public IAppDbContext _context { get; set; }
     public GetDashboardCardByIdsQueryHandler(IAppDbContext context) => _context = context;

     public async Task<GetDashboardCardByIdsQueryResponse> Handle(GetDashboardCardByIdsQueryRequest request, CancellationToken cancellationToken)
         => new GetDashboardCardByIdsQueryResponse()
         {
             DashboardCards = await _context.DashboardCards
             .Where(x => request.DashboardCardIds.Contains(x.DashboardCardId))
             .Select(x => DashboardCardApiModel.FromDashboardCard(x)).ToListAsync()
         };
 }
