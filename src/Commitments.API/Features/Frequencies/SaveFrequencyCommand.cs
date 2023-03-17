using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.Frequencies;

 public class SaveFrequencyCommandValidator: AbstractValidator<SaveFrequencyCommandRequest> {
     public SaveFrequencyCommandValidator()
     {
         RuleFor(request => request.Frequency.FrequencyId).NotNull();
     }
 }

 public class SaveFrequencyCommandRequest : IRequest<SaveFrequencyCommandResponse> {
     public FrequencyDto Frequency { get; set; }
 }

 public class SaveFrequencyCommandResponse
 {            
     public int FrequencyId { get; set; }
 }

 public class SaveFrequencyCommandHandler : IRequestHandler<SaveFrequencyCommandRequest, SaveFrequencyCommandResponse>
 {
     public IAppDbContext _context { get; set; }

     public SaveFrequencyCommandHandler(IAppDbContext context) => _context = context;

     public async Task<SaveFrequencyCommandResponse> Handle(SaveFrequencyCommandRequest request, CancellationToken cancellationToken)
     {
         var frequency = await _context.Frequencies
             .Include(x => x.FrequencyType)
             .SingleOrDefaultAsync(x => x.FrequencyId == request.Frequency.FrequencyId);

         if (frequency == null) _context.Frequencies.Add(frequency = new Frequency());

         frequency.Frequency = request.Frequency.Frequency;

         frequency.FrequencyTypeId = request.Frequency.FrequencyTypeId;

         await _context.SaveChangesAsync(cancellationToken);

         return new SaveFrequencyCommandResponse() { FrequencyId = frequency.FrequencyId };
     }
 }
