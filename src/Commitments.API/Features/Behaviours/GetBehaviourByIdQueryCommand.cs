using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.Behaviours;

 public class GetBehaviourByIdQueryCommandValidator : AbstractValidator<GetBehaviourByIdQueryCommandRequest>
 {
     public GetBehaviourByIdQueryCommandValidator()
     {
         RuleFor(request => request.BehaviourId).NotEqual(0);
     }
 }

 public class GetBehaviourByIdQueryCommandRequest : IRequest<GetBehaviourByIdQueryCommandResponse> {
     public int BehaviourId { get; set; }
 }

 public class GetBehaviourByIdQueryCommandResponse
 {
     public BehaviourApiModel Behaviour { get; set; }
 }

 public class GetBehaviourByIdQueryCommandHandler : IRequestHandler<GetBehaviourByIdQueryCommandRequest, GetBehaviourByIdQueryCommandResponse>
 {
     public IAppDbContext _context { get; set; }

     public GetBehaviourByIdQueryCommandHandler(IAppDbContext context) => _context = context;

     public async Task<GetBehaviourByIdQueryCommandResponse> Handle(GetBehaviourByIdQueryCommandRequest request, CancellationToken cancellationToken)
         => new GetBehaviourByIdQueryCommandResponse()
         {
             Behaviour = BehaviourApiModel.FromBehaviour(await _context.Behaviours
                 .Include(x => x.BehaviourType)
                 .SingleAsync(x => x.BehaviourId == request.BehaviourId))
         };
 }
