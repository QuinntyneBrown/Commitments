using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Api.Features.Dashboards;

 public class GetDashboardByIdQueryCommandValidator : AbstractValidator<GetDashboardByIdQueryCommandRequest>
 {
     public GetDashboardByIdQueryCommandValidator()
     {
         RuleFor(request => request.DashboardId).NotEqual(0);
     }
 }

 public class GetDashboardByIdQueryCommandRequest : IRequest<GetDashboardByIdQueryCommandResponse> {
     public int DashboardId { get; set; }
 }

 public class GetDashboardByIdQueryCommandResponse
 {
     public DashboardApiModel Dashboard { get; set; }
 }

 public class GetDashboardByIdQueryCommandHandler : IRequestHandler<GetDashboardByIdQueryCommandRequest, GetDashboardByIdQueryCommandResponse>
 {
     public IAppDbContext _context { get; set; }

     public GetDashboardByIdQueryCommandHandler(IAppDbContext context) => _context = context;

     public async Task<GetDashboardByIdQueryCommandResponse> Handle(GetDashboardByIdQueryCommandRequest request, CancellationToken cancellationToken)
         => new GetDashboardByIdQueryCommandResponse()
         {
             Dashboard = DashboardApiModel.FromDashboard(await _context.Dashboards.FindAsync(request.DashboardId))
         };
 }
