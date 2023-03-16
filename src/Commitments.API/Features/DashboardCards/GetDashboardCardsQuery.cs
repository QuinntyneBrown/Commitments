using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.DashboardCards;

 public class GetDashboardCardsQueryRequest : IRequest<GetDashboardCardsQueryResponse> { }

 public class GetDashboardCardsQueryResponse
 {
     public IEnumerable<DashboardCardApiModel> DashboardCards { get; set; }
 }

 public class GetDashboardCardsQueryHandler : IRequestHandler<GetDashboardCardsQueryRequest, GetDashboardCardsQueryResponse>
 {
     public IAppDbContext _context { get; set; }

     public GetDashboardCardsQueryHandler(IAppDbContext context) => _context = context;

     public async Task<GetDashboardCardsQueryResponse> Handle(GetDashboardCardsQueryRequest request, CancellationToken cancellationToken)
         => new GetDashboardCardsQueryResponse()
         {
             DashboardCards = await _context.DashboardCards.Select(x => DashboardCardApiModel.FromDashboardCard(x)).ToListAsync()
         };
 }
