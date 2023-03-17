using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.AggregateModel;
using Commitments.Core.Interfaces;


namespace Commitments.Api.Features.Dashboards;

 public class SaveDashboardCommandValidator: AbstractValidator<SaveDashboardCommandRequest> {
     public SaveDashboardCommandValidator()
     {
         RuleFor(request => request.Dashboard.DashboardId).NotNull();
     }
 }

 public class SaveDashboardCommandRequest : IRequest<SaveDashboardCommandResponse> {
     public DashboardDto Dashboard { get; set; }
 }

 public class SaveDashboardCommandResponse
 {            
     public int DashboardId { get; set; }
 }

 public class SaveDashboardCommandHandler : IRequestHandler<SaveDashboardCommandRequest, SaveDashboardCommandResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public SaveDashboardCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task<SaveDashboardCommandResponse> Handle(SaveDashboardCommandRequest request, CancellationToken cancellationToken)
     {
         var dashboard = await _context.Dashboards.FindAsync(request.Dashboard.DashboardId);

         if (dashboard == null) _context.Dashboards.Add(dashboard = new Dashboard());

         dashboard.Name = request.Dashboard.Name;

         dashboard.ProfileId = request.Dashboard.ProfileId;

         await _context.SaveChangesAsync(cancellationToken);

         return new SaveDashboardCommandResponse() { DashboardId = dashboard.DashboardId };
     }
 }
