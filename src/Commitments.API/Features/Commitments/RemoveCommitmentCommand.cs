using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;


namespace Commitments.Api.Features.Commitments;

 public class RemoveCommitmentCommandValidator : AbstractValidator<RemoveCommitmentCommandRequest>
 {
     public RemoveCommitmentCommandValidator()
     {
         RuleFor(request => request.CommitmentId).NotEqual(0);
     }
 }

 public class RemoveCommitmentCommandRequest : IRequest
 {
     public int CommitmentId { get; set; }
 }

 public class RemoveCommitmentCommandHandler : IRequestHandler<RemoveCommitmentCommandRequest>
 {
     public ICommimentsDbContext _context { get; set; }

     public RemoveCommitmentCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task Handle(RemoveCommitmentCommandRequest request, CancellationToken cancellationToken)
     {
         _context.Commitments.Remove(await _context.Commitments.FindAsync(request.CommitmentId));
         await _context.SaveChangesAsync(cancellationToken);
     }

 }
