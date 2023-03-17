using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;


namespace Commitments.Api.Features.Cards;

 public class RemoveCardCommandValidator : AbstractValidator<RemoveCardCommandRequest>
 {
     public RemoveCardCommandValidator()
     {
         RuleFor(request => request.CardId).NotEqual(0);
     }
 }

 public class RemoveCardCommandRequest : IRequest
 {
     public int CardId { get; set; }
 }

 public class RemoveCardCommandHandler : IRequestHandler<RemoveCardCommandRequest>
 {
     public ICommimentsDbContext _context { get; set; }

     public RemoveCardCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task Handle(RemoveCardCommandRequest request, CancellationToken cancellationToken)
     {
         _context.Cards.Remove(await _context.Cards.FindAsync(request.CardId));
         await _context.SaveChangesAsync(cancellationToken);
     }

 }
