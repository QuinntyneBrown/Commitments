using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;


namespace Commitments.Api.Features.DashboardCards;

 public class RemoveDashboardCardCommandValidator : AbstractValidator<RemoveDashboardCardCommandRequest>
 {
     public RemoveDashboardCardCommandValidator()
     {
         RuleFor(request => request.DashboardCardId).NotEqual(0);
     }
 }

 public class RemoveDashboardCardCommandRequest : IRequest
 {
     public int DashboardCardId { get; set; }
 }

 public class RemoveDashboardCardCommandHandler : IRequestHandler<RemoveDashboardCardCommandRequest>
 {
     public IAppDbContext _context { get; set; }

     public RemoveDashboardCardCommandHandler(IAppDbContext context) => _context = context;

     public async Task Handle(RemoveDashboardCardCommandRequest request, CancellationToken cancellationToken)
     {
         _context.DashboardCards.Remove(await _context.DashboardCards.FindAsync(request.DashboardCardId));
         await _context.SaveChangesAsync(cancellationToken);
     }

 }
