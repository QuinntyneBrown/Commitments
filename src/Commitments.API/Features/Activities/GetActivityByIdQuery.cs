using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.Activities;

 public class GetActivityByIdQueryCommandValidator : AbstractValidator<GetActivityByIdQueryRequest>
 {
     public GetActivityByIdQueryCommandValidator()
     {
         RuleFor(request => request.ActivityId).NotEqual(0);
     }
 }

 public class GetActivityByIdQueryRequest : IRequest<GetActivityByIdQueryResponse> {
     public int ActivityId { get; set; }
 }

 public class GetActivityByIdQueryResponse
 {
     public ActivityDto Activity { get; set; }
 }

 public class GetActivityByIdQueryCommandHandler : IRequestHandler<GetActivityByIdQueryRequest, GetActivityByIdQueryResponse>
 {
     public IAppDbContext _context { get; set; }

     public GetActivityByIdQueryCommandHandler(IAppDbContext context) => _context = context;

     public async Task<GetActivityByIdQueryResponse> Handle(GetActivityByIdQueryRequest request, CancellationToken cancellationToken)
         => new GetActivityByIdQueryResponse()
         {
             Activity = ActivityDto.FromActivity(await _context.Activities
                 .Include(x => x.Behaviour)
                 .Include("Behaviour.BehaviourType")
                 .SingleAsync(x => x.ActivityId == request.ActivityId))
         };
 }
