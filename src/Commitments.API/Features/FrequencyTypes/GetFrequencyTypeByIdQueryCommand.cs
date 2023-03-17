using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Api.Features.FrequencyTypes;

 public class GetFrequencyTypeByIdQueryCommandValidator : AbstractValidator<GetFrequencyTypeByIdQueryRequest>
 {
     public GetFrequencyTypeByIdQueryCommandValidator()
     {
         RuleFor(request => request.FrequencyTypeId).NotEqual(0);
     }
 }

 public class GetFrequencyTypeByIdQueryRequest : IRequest<GetFrequencyTypeByIdQueryResponse> {
     public int FrequencyTypeId { get; set; }
 }

 public class GetFrequencyTypeByIdQueryResponse
 {
     public FrequencyTypeDto FrequencyType { get; set; }
 }

 public class GetFrequencyTypeByIdQueryCommandHandler : IRequestHandler<GetFrequencyTypeByIdQueryRequest, GetFrequencyTypeByIdQueryResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public GetFrequencyTypeByIdQueryCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetFrequencyTypeByIdQueryResponse> Handle(GetFrequencyTypeByIdQueryRequest request, CancellationToken cancellationToken)
         => new GetFrequencyTypeByIdQueryResponse()
         {
             FrequencyType = FrequencyTypeDto.FromFrequencyType(await _context.FrequencyTypes.FindAsync(request.FrequencyTypeId))
         };
 }
