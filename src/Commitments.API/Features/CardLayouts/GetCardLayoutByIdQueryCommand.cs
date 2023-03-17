using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Api.Features.CardLayouts;

 public class GetCardLayoutByIdQueryCommandValidator : AbstractValidator<GetCardLayoutByIdQueryRequest>
 {
     public GetCardLayoutByIdQueryCommandValidator()
     {
         RuleFor(request => request.CardLayoutId).NotEqual(0);
     }
 }

 public class GetCardLayoutByIdQueryRequest : IRequest<GetCardLayoutByIdQueryResponse> {
     public int CardLayoutId { get; set; }
 }

 public class GetCardLayoutByIdQueryResponse
 {
     public CardLayoutDto CardLayout { get; set; }
 }

 public class GetCardLayoutByIdQueryCommandHandler : IRequestHandler<GetCardLayoutByIdQueryRequest, GetCardLayoutByIdQueryResponse>
 {
     public IAppDbContext _context { get; set; }


     public async Task<GetCardLayoutByIdQueryResponse> Handle(GetCardLayoutByIdQueryRequest request, CancellationToken cancellationToken)
         => new GetCardLayoutByIdQueryResponse()
         {
             CardLayout = CardLayoutDto.FromCardLayout(await _context.CardLayouts.FindAsync(request.CardLayoutId))
         };
 }
