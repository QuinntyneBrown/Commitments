using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Api.Features.BehaviourTypes;

 public class GetBehaviourTypeByIdValidator : AbstractValidator<GetBehaviourTypeByIdRequest>
 {
     public GetBehaviourTypeByIdValidator()
     {
         RuleFor(request => request.BehaviourTypeId).NotEqual(0);
     }
 }

 public class GetBehaviourTypeByIdRequest : IRequest<GetBehaviourTypeByIdResponse> {
     public int BehaviourTypeId { get; set; }
 }

 public class GetBehaviourTypeByIdResponse
 {
     public BehaviourTypeDto BehaviourType { get; set; }
 }

 public class GetBehaviourTypeByIdHandler : IRequestHandler<GetBehaviourTypeByIdRequest, GetBehaviourTypeByIdResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public GetBehaviourTypeByIdHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetBehaviourTypeByIdResponse> Handle(GetBehaviourTypeByIdRequest request, CancellationToken cancellationToken)
         => new GetBehaviourTypeByIdResponse()
         {
             BehaviourType = BehaviourTypeDto.FromBehaviourType(await _context.BehaviourTypes.FindAsync(request.BehaviourTypeId))
         };
 }
