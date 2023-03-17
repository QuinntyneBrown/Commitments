using Commitments.Core.AggregateModel;
using Commitments.Core.Interfaces;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;


namespace Commitments.Api.Features.BehaviourTypes;

 public class SaveBehaviourTypeCommandValidator: AbstractValidator<SaveBehaviourTypeCommandRequest> {
     public SaveBehaviourTypeCommandValidator()
     {
         RuleFor(request => request.BehaviourType.BehaviourTypeId).NotNull();
     }
 }

 public class SaveBehaviourTypeCommandRequest : IRequest<SaveBehaviourTypeCommandResponse> {
     public BehaviourTypeDto BehaviourType { get; set; }
 }

 public class SaveBehaviourTypeCommandResponse
 {            
     public int BehaviourTypeId { get; set; }
 }

 public class SaveBehaviourTypeCommandHandler : IRequestHandler<SaveBehaviourTypeCommandRequest, SaveBehaviourTypeCommandResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public SaveBehaviourTypeCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task<SaveBehaviourTypeCommandResponse> Handle(SaveBehaviourTypeCommandRequest request, CancellationToken cancellationToken)
     {
         var behaviourType = await _context.BehaviourTypes.FindAsync(request.BehaviourType.BehaviourTypeId);

         if (behaviourType == null) _context.BehaviourTypes.Add(behaviourType = new BehaviourType());

         behaviourType.Name = request.BehaviourType.Name;

         await _context.SaveChangesAsync(cancellationToken);

         return new SaveBehaviourTypeCommandResponse() { BehaviourTypeId = behaviourType.BehaviourTypeId };
     }
 }
