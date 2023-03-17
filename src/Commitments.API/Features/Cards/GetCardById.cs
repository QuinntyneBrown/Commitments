using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Api.Features.Cards;

 public class GetCardByIdValidator : AbstractValidator<GetCardByIdRequest>
 {
     public GetCardByIdValidator()
     {
         RuleFor(request => request.CardId).NotEqual(0);
     }
 }

 public class GetCardByIdRequest : IRequest<GetCardByIdResponse> {
     public int CardId { get; set; }
 }

 public class GetCardByIdResponse
 {
     public CardDto Card { get; set; }
 }

 public class GetCardByIdHandler : IRequestHandler<GetCardByIdRequest, GetCardByIdResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public GetCardByIdHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetCardByIdResponse> Handle(GetCardByIdRequest request, CancellationToken cancellationToken)
         => new GetCardByIdResponse()
         {
             Card = CardDto.FromCard(await _context.Cards.FindAsync(request.CardId))
         };
 }
