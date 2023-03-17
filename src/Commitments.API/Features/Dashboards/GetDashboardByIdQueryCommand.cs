using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Api.Features.Dashboards;

 public class GetDashboardByIdQueryCommandValidator : AbstractValidator<GetDashboardByIdQueryRequest>
 {
     public GetDashboardByIdQueryCommandValidator()
     {
         RuleFor(request => request.DashboardId).NotEqual(0);
     }
 }

 public class GetDashboardByIdQueryRequest : IRequest<GetDashboardByIdQueryResponse> {
     public int DashboardId { get; set; }
 }

 public class GetDashboardByIdQueryResponse
 {
     public DashboardDto Dashboard { get; set; }
 }

 public class GetDashboardByIdQueryCommandHandler : IRequestHandler<GetDashboardByIdQueryRequest, GetDashboardByIdQueryResponse>
 {
     public IAppDbContext _context { get; set; }

     public GetDashboardByIdQueryCommandHandler(IAppDbContext context) => _context = context;

     public async Task<GetDashboardByIdQueryResponse> Handle(GetDashboardByIdQueryRequest request, CancellationToken cancellationToken)
         => new GetDashboardByIdQueryResponse()
         {
             Dashboard = DashboardDto.FromDashboard(await _context.Dashboards.FindAsync(request.DashboardId))
         };
 }
