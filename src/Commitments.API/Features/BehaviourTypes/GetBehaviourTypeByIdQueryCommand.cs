using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Api.Features.BehaviourTypes;

 public class GetBehaviourTypeByIdQueryCommandValidator : AbstractValidator<GetBehaviourTypeByIdQueryRequest>
 {
     public GetBehaviourTypeByIdQueryCommandValidator()
     {
         RuleFor(request => request.BehaviourTypeId).NotEqual(0);
     }
 }

 public class GetBehaviourTypeByIdQueryRequest : IRequest<GetBehaviourTypeByIdQueryResponse> {
     public int BehaviourTypeId { get; set; }
 }

 public class GetBehaviourTypeByIdQueryResponse
 {
     public BehaviourTypeDto BehaviourType { get; set; }
 }

 public class GetBehaviourTypeByIdQueryCommandHandler : IRequestHandler<GetBehaviourTypeByIdQueryRequest, GetBehaviourTypeByIdQueryResponse>
 {
     public IAppDbContext _context { get; set; }

     public GetBehaviourTypeByIdQueryCommandHandler(IAppDbContext context) => _context = context;

     public async Task<GetBehaviourTypeByIdQueryResponse> Handle(GetBehaviourTypeByIdQueryRequest request, CancellationToken cancellationToken)
         => new GetBehaviourTypeByIdQueryResponse()
         {
             BehaviourType = BehaviourTypeDto.FromBehaviourType(await _context.BehaviourTypes.FindAsync(request.BehaviourTypeId))
         };
 }
