using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Api.Features.CardLayouts;

 public class GetCardLayoutByIdQueryCommandValidator : AbstractValidator<GetCardLayoutByIdQueryCommandRequest>
 {
     public GetCardLayoutByIdQueryCommandValidator()
     {
         RuleFor(request => request.CardLayoutId).NotEqual(0);
     }
 }

 public class GetCardLayoutByIdQueryCommandRequest : IRequest<GetCardLayoutByIdQueryCommandResponse> {
     public int CardLayoutId { get; set; }
 }

 public class GetCardLayoutByIdQueryCommandResponse
 {
     public CardLayoutApiModel CardLayout { get; set; }
 }

 public class GetCardLayoutByIdQueryCommandHandler : IRequestHandler<GetCardLayoutByIdQueryCommandRequest, GetCardLayoutByIdQueryCommandResponse>
 {
     public IAppDbContext _context { get; set; }


     public async Task<GetCardLayoutByIdQueryCommandResponse> Handle(GetCardLayoutByIdQueryCommandRequest request, CancellationToken cancellationToken)
         => new GetCardLayoutByIdQueryCommandResponse()
         {
             CardLayout = CardLayoutApiModel.FromCardLayout(await _context.CardLayouts.FindAsync(request.CardLayoutId))
         };
 }
