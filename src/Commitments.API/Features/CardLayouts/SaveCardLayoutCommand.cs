using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.AggregateModel;
using Commitments.Core.Interfaces;


namespace Commitments.Api.Features.CardLayouts;

 public class SaveCardLayoutCommandValidator: AbstractValidator<SaveCardLayoutCommandRequest> {
     public SaveCardLayoutCommandValidator()
     {
         RuleFor(request => request.CardLayout.CardLayoutId).NotNull();
     }
 }

 public class SaveCardLayoutCommandRequest : IRequest<SaveCardLayoutCommandResponse> {
     public CardLayoutDto CardLayout { get; set; }
 }

 public class SaveCardLayoutCommandResponse
 {			
     public int CardLayoutId { get; set; }
 }

 public class SaveCardLayoutCommandHandler : IRequestHandler<SaveCardLayoutCommandRequest, SaveCardLayoutCommandResponse>
 {
     public ICommimentsDbContext _context { get; set; }


     public async Task<SaveCardLayoutCommandResponse> Handle(SaveCardLayoutCommandRequest request, CancellationToken cancellationToken)
     {
         var cardLayout = await _context.CardLayouts.FindAsync(request.CardLayout.CardLayoutId);

         if (cardLayout == null) _context.CardLayouts.Add(cardLayout = new CardLayout());

         cardLayout.Name = request.CardLayout.Name;

         await _context.SaveChangesAsync(cancellationToken);

         return new SaveCardLayoutCommandResponse() { CardLayoutId = cardLayout.CardLayoutId };
     }
 }
