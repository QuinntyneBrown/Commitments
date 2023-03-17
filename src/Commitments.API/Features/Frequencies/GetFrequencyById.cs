using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Api.Features.Frequencies;

 public class GetFrequencyByIdValidator : AbstractValidator<GetFrequencyByIdRequest>
 {
     public GetFrequencyByIdValidator()
     {
         RuleFor(request => request.FrequencyId).NotEqual(0);
     }
 }

 public class GetFrequencyByIdRequest : IRequest<GetFrequencyByIdResponse> {
     public int FrequencyId { get; set; }
 }

 public class GetFrequencyByIdResponse
 {
     public FrequencyDto Frequency { get; set; }
 }

 public class GetFrequencyByIdHandler : IRequestHandler<GetFrequencyByIdRequest, GetFrequencyByIdResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public GetFrequencyByIdHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetFrequencyByIdResponse> Handle(GetFrequencyByIdRequest request, CancellationToken cancellationToken)
         => new GetFrequencyByIdResponse()
         {
             Frequency = FrequencyDto.FromFrequency(await _context.Frequencies.FindAsync(request.FrequencyId))
         };
 }
