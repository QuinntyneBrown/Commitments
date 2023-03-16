using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Api.Features.Cards;

 public class GetCardByIdQueryCommandValidator : AbstractValidator<GetCardByIdQueryCommandRequest>
 {
     public GetCardByIdQueryCommandValidator()
     {
         RuleFor(request => request.CardId).NotEqual(0);
     }
 }

 public class GetCardByIdQueryCommandRequest : IRequest<GetCardByIdQueryCommandResponse> {
     public int CardId { get; set; }
 }

 public class GetCardByIdQueryCommandResponse
 {
     public CardApiModel Card { get; set; }
 }

 public class GetCardByIdQueryCommandHandler : IRequestHandler<GetCardByIdQueryCommandRequest, GetCardByIdQueryCommandResponse>
 {
     public IAppDbContext _context { get; set; }

     public GetCardByIdQueryCommandHandler(IAppDbContext context) => _context = context;

     public async Task<GetCardByIdQueryCommandResponse> Handle(GetCardByIdQueryCommandRequest request, CancellationToken cancellationToken)
         => new GetCardByIdQueryCommandResponse()
         {
             Card = CardApiModel.FromCard(await _context.Cards.FindAsync(request.CardId))
         };
 }
