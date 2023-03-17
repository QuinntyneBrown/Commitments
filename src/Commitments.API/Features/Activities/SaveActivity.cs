using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.AggregateModel;
using Commitments.Core.Interfaces;


namespace Commitments.Api.Features.Activities;

 public class SaveActivityCommandValidator: AbstractValidator<SaveActivityRequest> {
     public SaveActivityCommandValidator()
     {
         RuleFor(request => request.Activity.ActivityId).NotNull();
     }
 }

 public class SaveActivityRequest : IRequest<SaveActivityResponse> {
     public ActivityDto Activity { get; set; }
 }

 public class SaveActivityResponse
 {            
     public int ActivityId { get; set; }
 }

 public class SaveActivityCommandHandler : IRequestHandler<SaveActivityRequest, SaveActivityResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public SaveActivityCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task<SaveActivityResponse> Handle(SaveActivityRequest request, CancellationToken cancellationToken)
     {
         var activity = await _context.Activities.FindAsync(request.Activity.ActivityId);

         if (activity == null) _context.Activities.Add(activity = new Activity());

         activity.BehaviourId = request.Activity.BehaviourId;
         activity.ProfileId = request.Activity.ProfileId;
         activity.PerformedOn = request.Activity.PerformedOn;
         activity.Description = request.Activity.Description;

         await _context.SaveChangesAsync(cancellationToken);

         return new SaveActivityResponse() { ActivityId = activity.ActivityId };
     }
 }
