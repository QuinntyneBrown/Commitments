using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;


namespace Commitments.Api.Features.Activities;

 public class SaveActivityCommandValidator: AbstractValidator<SaveActivityCommandRequest> {
     public SaveActivityCommandValidator()
     {
         RuleFor(request => request.Activity.ActivityId).NotNull();
     }
 }

 public class SaveActivityCommandRequest : IRequest<SaveActivityCommandResponse> {
     public ActivityDto Activity { get; set; }
 }

 public class SaveActivityCommandResponse
 {            
     public int ActivityId { get; set; }
 }

 public class SaveActivityCommandHandler : IRequestHandler<SaveActivityCommandRequest, SaveActivityCommandResponse>
 {
     public IAppDbContext _context { get; set; }

     public SaveActivityCommandHandler(IAppDbContext context) => _context = context;

     public async Task<SaveActivityCommandResponse> Handle(SaveActivityCommandRequest request, CancellationToken cancellationToken)
     {
         var activity = await _context.Activities.FindAsync(request.Activity.ActivityId);

         if (activity == null) _context.Activities.Add(activity = new Activity());

         activity.BehaviourId = request.Activity.BehaviourId;
         activity.ProfileId = request.Activity.ProfileId;
         activity.PerformedOn = request.Activity.PerformedOn;
         activity.Description = request.Activity.Description;

         await _context.SaveChangesAsync(cancellationToken);

         return new SaveActivityCommandResponse() { ActivityId = activity.ActivityId };
     }
 }
