using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;


namespace Commitments.Api.Features.Frequencies;

 public class RemoveFrequencyCommandValidator : AbstractValidator<RemoveFrequencyCommandRequest>
 {
     public RemoveFrequencyCommandValidator()
     {
         RuleFor(request => request.FrequencyId).NotEqual(0);
     }
 }

 public class RemoveFrequencyCommandRequest : IRequest
 {
     public int FrequencyId { get; set; }
 }

 public class RemoveFrequencyCommandHandler : IRequestHandler<RemoveFrequencyCommandRequest>
 {
     public IAppDbContext _context { get; set; }

     public RemoveFrequencyCommandHandler(IAppDbContext context) => _context = context;

     public async Task Handle(RemoveFrequencyCommandRequest request, CancellationToken cancellationToken)
     {
         _context.Frequencies.Remove(await _context.Frequencies.FindAsync(request.FrequencyId));
         await _context.SaveChangesAsync(cancellationToken);
     }

 }
