// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.AggregateModel;
using Commitments.Core.Interfaces;


namespace Commitments.Api.Features.Cards;

 public class SaveCardCommandValidator: AbstractValidator<SaveCardRequest> {
     public SaveCardCommandValidator()
     {
         RuleFor(request => request.Card.CardId).NotNull();
     }
 }

 public class SaveCardRequest : IRequest<SaveCardResponse> {
     public CardDto Card { get; set; }
 }

 public class SaveCardResponse
 {            
     public int CardId { get; set; }
 }

 public class SaveCardCommandHandler : IRequestHandler<SaveCardRequest, SaveCardResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public SaveCardCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task<SaveCardResponse> Handle(SaveCardRequest request, CancellationToken cancellationToken)
     {
         var card = await _context.Cards.FindAsync(request.Card.CardId);

         if (card == null) _context.Cards.Add(card = new Card());

         card.Name = request.Card.Name;
         card.Description = request.Card.Description;

         await _context.SaveChangesAsync(cancellationToken);

         return new SaveCardResponse() { CardId = card.CardId };
     }
 }

