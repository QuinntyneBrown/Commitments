using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;


namespace Commitments.Api.Features.Behaviours;

 public class RemoveBehaviourCommandValidator : AbstractValidator<RemoveBehaviourCommandRequest>
 {
     public RemoveBehaviourCommandValidator()
     {
         RuleFor(request => request.BehaviourId).NotEqual(0);
     }
 }

 public class RemoveBehaviourCommandRequest : IRequest
 {
     public int BehaviourId { get; set; }
 }

 public class RemoveBehaviourCommandHandler : IRequestHandler<RemoveBehaviourCommandRequest>
 {
     public IAppDbContext _context { get; set; }

     public RemoveBehaviourCommandHandler(IAppDbContext context) => _context = context;

     public async Task Handle(RemoveBehaviourCommandRequest request, CancellationToken cancellationToken)
     {
         _context.Behaviours.Remove(await _context.Behaviours.FindAsync(request.BehaviourId));
         await _context.SaveChangesAsync(cancellationToken);
     }

 }
