// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel;
using Commitments.Core.Interfaces;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;


namespace Commitments.Api.Features.BehaviourTypes;

 public class SaveBehaviourTypeCommandValidator: AbstractValidator<SaveBehaviourTypeRequest> {
     public SaveBehaviourTypeCommandValidator()
     {
         RuleFor(request => request.BehaviourType.BehaviourTypeId).NotNull();
     }
 }

 public class SaveBehaviourTypeRequest : IRequest<SaveBehaviourTypeResponse> {
     public BehaviourTypeDto BehaviourType { get; set; }
 }

 public class SaveBehaviourTypeResponse
 {            
     public int BehaviourTypeId { get; set; }
 }

 public class SaveBehaviourTypeCommandHandler : IRequestHandler<SaveBehaviourTypeRequest, SaveBehaviourTypeResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public SaveBehaviourTypeCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task<SaveBehaviourTypeResponse> Handle(SaveBehaviourTypeRequest request, CancellationToken cancellationToken)
     {
         var behaviourType = await _context.BehaviourTypes.FindAsync(request.BehaviourType.BehaviourTypeId);

         if (behaviourType == null) _context.BehaviourTypes.Add(behaviourType = new BehaviourType());

         behaviourType.Name = request.BehaviourType.Name;

         await _context.SaveChangesAsync(cancellationToken);

         return new SaveBehaviourTypeResponse() { BehaviourTypeId = behaviourType.BehaviourTypeId };
     }
 }

