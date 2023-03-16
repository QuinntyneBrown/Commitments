using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Api.Features.BehaviourTypes;

 public class GetBehaviourTypeByIdQueryCommandValidator : AbstractValidator<GetBehaviourTypeByIdQueryCommandRequest>
 {
     public GetBehaviourTypeByIdQueryCommandValidator()
     {
         RuleFor(request => request.BehaviourTypeId).NotEqual(0);
     }
 }

 public class GetBehaviourTypeByIdQueryCommandRequest : IRequest<GetBehaviourTypeByIdQueryCommandResponse> {
     public int BehaviourTypeId { get; set; }
 }

 public class GetBehaviourTypeByIdQueryCommandResponse
 {
     public BehaviourTypeApiModel BehaviourType { get; set; }
 }

 public class GetBehaviourTypeByIdQueryCommandHandler : IRequestHandler<GetBehaviourTypeByIdQueryCommandRequest, GetBehaviourTypeByIdQueryCommandResponse>
 {
     public IAppDbContext _context { get; set; }

     public GetBehaviourTypeByIdQueryCommandHandler(IAppDbContext context) => _context = context;

     public async Task<GetBehaviourTypeByIdQueryCommandResponse> Handle(GetBehaviourTypeByIdQueryCommandRequest request, CancellationToken cancellationToken)
         => new GetBehaviourTypeByIdQueryCommandResponse()
         {
             BehaviourType = BehaviourTypeApiModel.FromBehaviourType(await _context.BehaviourTypes.FindAsync(request.BehaviourTypeId))
         };
 }
