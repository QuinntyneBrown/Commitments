using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.AggregateModel;
using Commitments.Core.Interfaces;


namespace Commitments.Api.Features.Cards;

 public class SaveCardCommandValidator: AbstractValidator<SaveCardCommandRequest> {
     public SaveCardCommandValidator()
     {
         RuleFor(request => request.Card.CardId).NotNull();
     }
 }

 public class SaveCardCommandRequest : IRequest<SaveCardCommandResponse> {
     public CardDto Card { get; set; }
 }

 public class SaveCardCommandResponse
 {            
     public int CardId { get; set; }
 }

 public class SaveCardCommandHandler : IRequestHandler<SaveCardCommandRequest, SaveCardCommandResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public SaveCardCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task<SaveCardCommandResponse> Handle(SaveCardCommandRequest request, CancellationToken cancellationToken)
     {
         var card = await _context.Cards.FindAsync(request.Card.CardId);

         if (card == null) _context.Cards.Add(card = new Card());

         card.Name = request.Card.Name;
         card.Description = request.Card.Description;

         await _context.SaveChangesAsync(cancellationToken);

         return new SaveCardCommandResponse() { CardId = card.CardId };
     }
 }
