using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.AggregateModel;
using Commitments.Core.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.Frequencies;

 public class SaveFrequencyCommandValidator: AbstractValidator<SaveFrequencyRequest> {
     public SaveFrequencyCommandValidator()
     {
         RuleFor(request => request.Frequency.FrequencyId).NotNull();
     }
 }

 public class SaveFrequencyRequest : IRequest<SaveFrequencyResponse> {
     public FrequencyDto Frequency { get; set; }
 }

 public class SaveFrequencyResponse
 {            
     public int FrequencyId { get; set; }
 }

 public class SaveFrequencyCommandHandler : IRequestHandler<SaveFrequencyRequest, SaveFrequencyResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public SaveFrequencyCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task<SaveFrequencyResponse> Handle(SaveFrequencyRequest request, CancellationToken cancellationToken)
     {
         var frequency = await _context.Frequencies
             .Include(x => x.FrequencyType)
             .SingleOrDefaultAsync(x => x.FrequencyId == request.Frequency.FrequencyId);

         if (frequency == null) _context.Frequencies.Add(frequency = new Frequency());

         frequency.Frequency = request.Frequency.Frequency;

         frequency.FrequencyTypeId = request.Frequency.FrequencyTypeId;

         await _context.SaveChangesAsync(cancellationToken);

         return new SaveFrequencyResponse() { FrequencyId = frequency.FrequencyId };
     }
 }
