using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;


namespace Commitments.Api.Features.Commitments;

 public class RemoveCommitmentCommandValidator : AbstractValidator<RemoveCommitmentRequest>
 {
     public RemoveCommitmentCommandValidator()
     {
         RuleFor(request => request.CommitmentId).NotEqual(0);
     }
 }

 public class RemoveCommitmentRequest : IRequest
 {
     public int CommitmentId { get; set; }
 }

 public class RemoveCommitmentCommandHandler : IRequestHandler<RemoveCommitmentRequest>
 {
     public ICommimentsDbContext _context { get; set; }

     public RemoveCommitmentCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task Handle(RemoveCommitmentRequest request, CancellationToken cancellationToken)
     {
         _context.Commitments.Remove(await _context.Commitments.FindAsync(request.CommitmentId));
         await _context.SaveChangesAsync(cancellationToken);
     }

 }
