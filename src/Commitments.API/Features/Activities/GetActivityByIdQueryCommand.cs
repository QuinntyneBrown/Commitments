using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.Activities;

 public class GetActivityByIdQueryCommandValidator : AbstractValidator<GetActivityByIdQueryCommandRequest>
 {
     public GetActivityByIdQueryCommandValidator()
     {
         RuleFor(request => request.ActivityId).NotEqual(0);
     }
 }

 public class GetActivityByIdQueryCommandRequest : IRequest<GetActivityByIdQueryCommandResponse> {
     public int ActivityId { get; set; }
 }

 public class GetActivityByIdQueryCommandResponse
 {
     public ActivityApiModel Activity { get; set; }
 }

 public class GetActivityByIdQueryCommandHandler : IRequestHandler<GetActivityByIdQueryCommandRequest, GetActivityByIdQueryCommandResponse>
 {
     public IAppDbContext _context { get; set; }

     public GetActivityByIdQueryCommandHandler(IAppDbContext context) => _context = context;

     public async Task<GetActivityByIdQueryCommandResponse> Handle(GetActivityByIdQueryCommandRequest request, CancellationToken cancellationToken)
         => new GetActivityByIdQueryCommandResponse()
         {
             Activity = ActivityApiModel.FromActivity(await _context.Activities
                 .Include(x => x.Behaviour)
                 .Include("Behaviour.BehaviourType")
                 .SingleAsync(x => x.ActivityId == request.ActivityId))
         };
 }
