using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;


namespace Commitments.Api.Features.FrequencyTypes;

 public class SaveFrequencyTypeCommandValidator: AbstractValidator<SaveFrequencyTypeCommandRequest> {
     public SaveFrequencyTypeCommandValidator()
     {
         RuleFor(request => request.FrequencyType.FrequencyTypeId).NotNull();
     }
 }

 public class SaveFrequencyTypeCommandRequest : IRequest<SaveFrequencyTypeCommandResponse> {
     public FrequencyTypeDto FrequencyType { get; set; }
 }

 public class SaveFrequencyTypeCommandResponse
 {            
     public int FrequencyTypeId { get; set; }
 }

 public class SaveFrequencyTypeCommandHandler : IRequestHandler<SaveFrequencyTypeCommandRequest, SaveFrequencyTypeCommandResponse>
 {
     public IAppDbContext _context { get; set; }

     public SaveFrequencyTypeCommandHandler(IAppDbContext context) => _context = context;

     public async Task<SaveFrequencyTypeCommandResponse> Handle(SaveFrequencyTypeCommandRequest request, CancellationToken cancellationToken)
     {
         var frequencyType = await _context.FrequencyTypes.FindAsync(request.FrequencyType.FrequencyTypeId);

         if (frequencyType == null) _context.FrequencyTypes.Add(frequencyType = new FrequencyType());

         frequencyType.Name = request.FrequencyType.Name;

         await _context.SaveChangesAsync(cancellationToken);

         return new SaveFrequencyTypeCommandResponse() { FrequencyTypeId = frequencyType.FrequencyTypeId };
     }
 }
