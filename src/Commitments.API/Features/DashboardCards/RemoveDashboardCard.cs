using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;


namespace Commitments.Api.Features.DashboardCards;

 public class RemoveDashboardCardCommandValidator : AbstractValidator<RemoveDashboardCardRequest>
 {
     public RemoveDashboardCardCommandValidator()
     {
         RuleFor(request => request.DashboardCardId).NotEqual(0);
     }
 }

 public class RemoveDashboardCardRequest : IRequest
 {
     public int DashboardCardId { get; set; }
 }

 public class RemoveDashboardCardCommandHandler : IRequestHandler<RemoveDashboardCardRequest>
 {
     public ICommimentsDbContext _context { get; set; }

     public RemoveDashboardCardCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task Handle(RemoveDashboardCardRequest request, CancellationToken cancellationToken)
     {
         _context.DashboardCards.Remove(await _context.DashboardCards.FindAsync(request.DashboardCardId));
         await _context.SaveChangesAsync(cancellationToken);
     }

 }
