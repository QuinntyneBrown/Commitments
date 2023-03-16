using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Api.Features.FrequencyTypes;

 public class GetFrequencyTypeByIdQueryCommandValidator : AbstractValidator<GetFrequencyTypeByIdQueryCommandRequest>
 {
     public GetFrequencyTypeByIdQueryCommandValidator()
     {
         RuleFor(request => request.FrequencyTypeId).NotEqual(0);
     }
 }

 public class GetFrequencyTypeByIdQueryCommandRequest : IRequest<GetFrequencyTypeByIdQueryCommandResponse> {
     public int FrequencyTypeId { get; set; }
 }

 public class GetFrequencyTypeByIdQueryCommandResponse
 {
     public FrequencyTypeApiModel FrequencyType { get; set; }
 }

 public class GetFrequencyTypeByIdQueryCommandHandler : IRequestHandler<GetFrequencyTypeByIdQueryCommandRequest, GetFrequencyTypeByIdQueryCommandResponse>
 {
     public IAppDbContext _context { get; set; }

     public GetFrequencyTypeByIdQueryCommandHandler(IAppDbContext context) => _context = context;

     public async Task<GetFrequencyTypeByIdQueryCommandResponse> Handle(GetFrequencyTypeByIdQueryCommandRequest request, CancellationToken cancellationToken)
         => new GetFrequencyTypeByIdQueryCommandResponse()
         {
             FrequencyType = FrequencyTypeApiModel.FromFrequencyType(await _context.FrequencyTypes.FindAsync(request.FrequencyTypeId))
         };
 }
