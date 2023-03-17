using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.Dashboards;

 public class GetDashboardByProfileIdQueryRequest : IRequest<GetDashboardByProfileIdQueryResponse> {
     public int ProfileId { get; set; }
 }

 public class GetDashboardByProfileIdQueryResponse
 {
     public DashboardDto Dashboard { get; set; }
 }

 public class GetDashboardByProfileIdQueryHandler : IRequestHandler<GetDashboardByProfileIdQueryRequest, GetDashboardByProfileIdQueryResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public GetDashboardByProfileIdQueryHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetDashboardByProfileIdQueryResponse> Handle(GetDashboardByProfileIdQueryRequest request, CancellationToken cancellationToken)
         => new GetDashboardByProfileIdQueryResponse()
         {
             Dashboard = DashboardDto.FromDashboard(await _context.Dashboards
                 .Include(x => x.DashboardCards)
                 .SingleOrDefaultAsync(x => x.ProfileId == request.ProfileId))
         };
 }
