using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;


namespace Commitments.Api.Features.Dashboards;

 public class RemoveDashboardCommandValidator : AbstractValidator<RemoveDashboardCommandRequest>
 {
     public RemoveDashboardCommandValidator()
     {
         RuleFor(request => request.DashboardId).NotEqual(0);
     }
 }

 public class RemoveDashboardCommandRequest : IRequest
 {
     public int DashboardId { get; set; }
 }

 public class RemoveDashboardCommandHandler : IRequestHandler<RemoveDashboardCommandRequest>
 {
     public IAppDbContext _context { get; set; }

     public RemoveDashboardCommandHandler(IAppDbContext context) => _context = context;

     public async Task Handle(RemoveDashboardCommandRequest request, CancellationToken cancellationToken)
     {
         _context.Dashboards.Remove(await _context.Dashboards.FindAsync(request.DashboardId));
         await _context.SaveChangesAsync(cancellationToken);
     }

 }
