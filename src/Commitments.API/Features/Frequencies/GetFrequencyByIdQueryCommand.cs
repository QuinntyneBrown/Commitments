using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Api.Features.Frequencies;

 public class GetFrequencyByIdQueryCommandValidator : AbstractValidator<GetFrequencyByIdQueryCommandRequest>
 {
     public GetFrequencyByIdQueryCommandValidator()
     {
         RuleFor(request => request.FrequencyId).NotEqual(0);
     }
 }

 public class GetFrequencyByIdQueryCommandRequest : IRequest<GetFrequencyByIdQueryCommandResponse> {
     public int FrequencyId { get; set; }
 }

 public class GetFrequencyByIdQueryCommandResponse
 {
     public FrequencyApiModel Frequency { get; set; }
 }

 public class GetFrequencyByIdQueryCommandHandler : IRequestHandler<GetFrequencyByIdQueryCommandRequest, GetFrequencyByIdQueryCommandResponse>
 {
     public IAppDbContext _context { get; set; }

     public GetFrequencyByIdQueryCommandHandler(IAppDbContext context) => _context = context;

     public async Task<GetFrequencyByIdQueryCommandResponse> Handle(GetFrequencyByIdQueryCommandRequest request, CancellationToken cancellationToken)
         => new GetFrequencyByIdQueryCommandResponse()
         {
             Frequency = FrequencyApiModel.FromFrequency(await _context.Frequencies.FindAsync(request.FrequencyId))
         };
 }
