using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.AggregateModel;
using Commitments.Core.Interfaces;


namespace Commitments.Api.Features.Behaviours;

 public class RemoveBehaviourCommandValidator : AbstractValidator<RemoveBehaviourRequest>
 {
     public RemoveBehaviourCommandValidator()
     {
         RuleFor(request => request.BehaviourId).NotEqual(0);
     }
 }

 public class RemoveBehaviourRequest : IRequest
 {
     public int BehaviourId { get; set; }
 }

 public class RemoveBehaviourCommandHandler : IRequestHandler<RemoveBehaviourRequest>
 {
     public ICommimentsDbContext _context { get; set; }

     public RemoveBehaviourCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task Handle(RemoveBehaviourRequest request, CancellationToken cancellationToken)
     {
         _context.Behaviours.Remove(await _context.Behaviours.FindAsync(request.BehaviourId));
         await _context.SaveChangesAsync(cancellationToken);
     }

 }
