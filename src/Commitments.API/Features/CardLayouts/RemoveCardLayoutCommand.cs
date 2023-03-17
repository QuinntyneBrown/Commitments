using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.AggregateModel;
using Commitments.Core.Interfaces;


namespace Commitments.Api.Features.CardLayouts;

 public class RemoveCardLayoutCommandValidator : AbstractValidator<RemoveCardLayoutCommandRequest>
 {
     public RemoveCardLayoutCommandValidator()
     {
         RuleFor(request => request.CardLayoutId).NotEqual(0);
     }
 }

 public class RemoveCardLayoutCommandRequest : IRequest
 {
     public int CardLayoutId { get; set; }
 }

 public class RemoveCardLayoutCommandHandler : IRequestHandler<RemoveCardLayoutCommandRequest>
 {
     public ICommimentsDbContext _context { get; set; }


     public async Task Handle(RemoveCardLayoutCommandRequest request, CancellationToken cancellationToken)
     {
         _context.CardLayouts.Remove(await _context.CardLayouts.FindAsync(request.CardLayoutId));
         await _context.SaveChangesAsync(cancellationToken);
     }

 }
