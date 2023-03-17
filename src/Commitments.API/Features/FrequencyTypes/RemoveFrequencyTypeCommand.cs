using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.AggregateModel;
using Commitments.Core.Interfaces;


namespace Commitments.Api.Features.FrequencyTypes;

 public class RemoveFrequencyTypeCommandValidator : AbstractValidator<RemoveFrequencyTypeCommandRequest>
 {
     public RemoveFrequencyTypeCommandValidator()
     {
         RuleFor(request => request.FrequencyTypeId).NotEqual(0);
     }
 }

 public class RemoveFrequencyTypeCommandRequest : IRequest
 {
     public int FrequencyTypeId { get; set; }
 }

 public class RemoveFrequencyTypeCommandHandler : IRequestHandler<RemoveFrequencyTypeCommandRequest>
 {
     public ICommimentsDbContext _context { get; set; }

     public RemoveFrequencyTypeCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task Handle(RemoveFrequencyTypeCommandRequest request, CancellationToken cancellationToken)
     {
         _context.FrequencyTypes.Remove(await _context.FrequencyTypes.FindAsync(request.FrequencyTypeId));
         await _context.SaveChangesAsync(cancellationToken);
     }

 }
