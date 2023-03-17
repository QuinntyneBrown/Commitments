using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Api.Features.Cards;

 public class GetCardByIdQueryCommandValidator : AbstractValidator<GetCardByIdQueryRequest>
 {
     public GetCardByIdQueryCommandValidator()
     {
         RuleFor(request => request.CardId).NotEqual(0);
     }
 }

 public class GetCardByIdQueryRequest : IRequest<GetCardByIdQueryResponse> {
     public int CardId { get; set; }
 }

 public class GetCardByIdQueryResponse
 {
     public CardDto Card { get; set; }
 }

 public class GetCardByIdQueryCommandHandler : IRequestHandler<GetCardByIdQueryRequest, GetCardByIdQueryResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public GetCardByIdQueryCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetCardByIdQueryResponse> Handle(GetCardByIdQueryRequest request, CancellationToken cancellationToken)
         => new GetCardByIdQueryResponse()
         {
             Card = CardDto.FromCard(await _context.Cards.FindAsync(request.CardId))
         };
 }
