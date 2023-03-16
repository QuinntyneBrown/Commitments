using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;


namespace Commitments.Api.Features.BehaviourTypes;

 public class RemoveBehaviourTypeCommandValidator : AbstractValidator<RemoveBehaviourTypeCommandRequest>
 {
     public RemoveBehaviourTypeCommandValidator()
     {
         RuleFor(request => request.BehaviourTypeId).NotEqual(0);
     }
 }

 public class RemoveBehaviourTypeCommandRequest : IRequest
 {
     public int BehaviourTypeId { get; set; }
 }

 public class RemoveBehaviourTypeCommandHandler : IRequestHandler<RemoveBehaviourTypeCommandRequest>
 {
     public IAppDbContext _context { get; set; }

     public RemoveBehaviourTypeCommandHandler(IAppDbContext context) => _context = context;

     public async Task Handle(RemoveBehaviourTypeCommandRequest request, CancellationToken cancellationToken)
     {
         _context.BehaviourTypes.Remove(await _context.BehaviourTypes.FindAsync(request.BehaviourTypeId));
         await _context.SaveChangesAsync(cancellationToken);
     }

 }
