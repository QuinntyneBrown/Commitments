using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.Behaviours;

 public class GetBehaviourByIdQueryCommandValidator : AbstractValidator<GetBehaviourByIdQueryRequest>
 {
     public GetBehaviourByIdQueryCommandValidator()
     {
         RuleFor(request => request.BehaviourId).NotEqual(0);
     }
 }

 public class GetBehaviourByIdQueryRequest : IRequest<GetBehaviourByIdQueryResponse> {
     public int BehaviourId { get; set; }
 }

 public class GetBehaviourByIdQueryResponse
 {
     public BehaviourDto Behaviour { get; set; }
 }

 public class GetBehaviourByIdQueryCommandHandler : IRequestHandler<GetBehaviourByIdQueryRequest, GetBehaviourByIdQueryResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public GetBehaviourByIdQueryCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetBehaviourByIdQueryResponse> Handle(GetBehaviourByIdQueryRequest request, CancellationToken cancellationToken)
         => new GetBehaviourByIdQueryResponse()
         {
             Behaviour = BehaviourDto.FromBehaviour(await _context.Behaviours
                 .Include(x => x.BehaviourType)
                 .SingleAsync(x => x.BehaviourId == request.BehaviourId))
         };
 }
