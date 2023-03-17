using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Api.Features.Frequencies;

 public class GetFrequencyByIdQueryCommandValidator : AbstractValidator<GetFrequencyByIdQueryRequest>
 {
     public GetFrequencyByIdQueryCommandValidator()
     {
         RuleFor(request => request.FrequencyId).NotEqual(0);
     }
 }

 public class GetFrequencyByIdQueryRequest : IRequest<GetFrequencyByIdQueryResponse> {
     public int FrequencyId { get; set; }
 }

 public class GetFrequencyByIdQueryResponse
 {
     public FrequencyDto Frequency { get; set; }
 }

 public class GetFrequencyByIdQueryCommandHandler : IRequestHandler<GetFrequencyByIdQueryRequest, GetFrequencyByIdQueryResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public GetFrequencyByIdQueryCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetFrequencyByIdQueryResponse> Handle(GetFrequencyByIdQueryRequest request, CancellationToken cancellationToken)
         => new GetFrequencyByIdQueryResponse()
         {
             Frequency = FrequencyDto.FromFrequency(await _context.Frequencies.FindAsync(request.FrequencyId))
         };
 }
