// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.AggregateModel;
using Commitments.Core.Interfaces;


namespace Commitments.Api.Features.CardLayouts;

 public class SaveCardLayoutCommandValidator: AbstractValidator<SaveCardLayoutRequest> {
     public SaveCardLayoutCommandValidator()
     {
         RuleFor(request => request.CardLayout.CardLayoutId).NotNull();
     }
 }

 public class SaveCardLayoutRequest : IRequest<SaveCardLayoutResponse> {
     public CardLayoutDto CardLayout { get; set; }
 }

 public class SaveCardLayoutResponse
 {			
     public int CardLayoutId { get; set; }
 }

 public class SaveCardLayoutCommandHandler : IRequestHandler<SaveCardLayoutRequest, SaveCardLayoutResponse>
 {
     public ICommimentsDbContext _context { get; set; }


     public async Task<SaveCardLayoutResponse> Handle(SaveCardLayoutRequest request, CancellationToken cancellationToken)
     {
         var cardLayout = await _context.CardLayouts.FindAsync(request.CardLayout.CardLayoutId);

         if (cardLayout == null) _context.CardLayouts.Add(cardLayout = new CardLayout());

         cardLayout.Name = request.CardLayout.Name;

         await _context.SaveChangesAsync(cancellationToken);

         return new SaveCardLayoutResponse() { CardLayoutId = cardLayout.CardLayoutId };
     }
 }

