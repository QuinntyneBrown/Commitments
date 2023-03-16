using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;
using Commitments.Core.Extensions;


namespace Commitments.Api.Features.Behaviours;

 public class SaveBehaviourCommandValidator: AbstractValidator<SaveBehaviourCommandRequest> {
     public SaveBehaviourCommandValidator()
     {
         RuleFor(request => request.Behaviour.BehaviourId).NotNull();
     }
 }

 public class SaveBehaviourCommandRequest : IRequest<SaveBehaviourCommandResponse> {
     public BehaviourApiModel Behaviour { get; set; }
 }

 public class SaveBehaviourCommandResponse
 {            
     public int BehaviourId { get; set; }
 }

 public class SaveBehaviourCommandHandler : IRequestHandler<SaveBehaviourCommandRequest, SaveBehaviourCommandResponse>
 {
     public IAppDbContext _context { get; set; }

     public SaveBehaviourCommandHandler(IAppDbContext context) => _context = context;

     public async Task<SaveBehaviourCommandResponse> Handle(SaveBehaviourCommandRequest request, CancellationToken cancellationToken)
     {
         var behaviour = await _context.Behaviours.FindAsync(request.Behaviour.BehaviourId);

         if (behaviour == null) _context.Behaviours.Add(behaviour = new Behaviour());

         behaviour.Name = request.Behaviour.Name;
         behaviour.Slug = request.Behaviour.Name.GenerateSlug();
         behaviour.Description = request.Behaviour.Description;
         behaviour.BehaviourTypeId = request.Behaviour.BehaviourTypeId;
         await _context.SaveChangesAsync(cancellationToken);

         return new SaveBehaviourCommandResponse() { BehaviourId = behaviour.BehaviourId };
     }
 }
