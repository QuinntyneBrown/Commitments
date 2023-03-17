using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.Dashboards;

 public class GetDashboardByProfileIdRequest : IRequest<GetDashboardByProfileIdResponse> {
     public int ProfileId { get; set; }
 }

 public class GetDashboardByProfileIdResponse
 {
     public DashboardDto Dashboard { get; set; }
 }

 public class GetDashboardByProfileIdQueryHandler : IRequestHandler<GetDashboardByProfileIdRequest, GetDashboardByProfileIdResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public GetDashboardByProfileIdQueryHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetDashboardByProfileIdResponse> Handle(GetDashboardByProfileIdRequest request, CancellationToken cancellationToken)
         => new GetDashboardByProfileIdResponse()
         {
             Dashboard = DashboardDto.FromDashboard(await _context.Dashboards
                 .Include(x => x.DashboardCards)
                 .SingleOrDefaultAsync(x => x.ProfileId == request.ProfileId))
         };
 }
