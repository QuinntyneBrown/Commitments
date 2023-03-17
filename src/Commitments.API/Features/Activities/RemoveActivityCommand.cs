using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.AggregateModel;
using Commitments.Core.Interfaces;


namespace Commitments.Api.Features.Activities;

 public class RemoveActivityCommandValidator : AbstractValidator<RemoveActivityCommandRequest>
 {
     public RemoveActivityCommandValidator()
     {
         RuleFor(request => request.ActivityId).NotEqual(0);
     }
 }

 public class RemoveActivityCommandRequest : IRequest
 {
     public int ActivityId { get; set; }
 }

 public class RemoveActivityCommandHandler : IRequestHandler<RemoveActivityCommandRequest>
 {
     public ICommimentsDbContext _context { get; set; }

     public RemoveActivityCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task Handle(RemoveActivityCommandRequest request, CancellationToken cancellationToken)
     {
         _context.Activities.Remove(await _context.Activities.FindAsync(request.ActivityId));
         await _context.SaveChangesAsync(cancellationToken);
     }
 }
