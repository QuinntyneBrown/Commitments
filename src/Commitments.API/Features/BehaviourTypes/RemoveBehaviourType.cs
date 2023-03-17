// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.AggregateModel;
using Commitments.Core.Interfaces;


namespace Commitments.Api.Features.BehaviourTypes;

 public class RemoveBehaviourTypeCommandValidator : AbstractValidator<RemoveBehaviourTypeRequest>
 {
     public RemoveBehaviourTypeCommandValidator()
     {
         RuleFor(request => request.BehaviourTypeId).NotEqual(0);
     }
 }

 public class RemoveBehaviourTypeRequest : IRequest
 {
     public int BehaviourTypeId { get; set; }
 }

 public class RemoveBehaviourTypeCommandHandler : IRequestHandler<RemoveBehaviourTypeRequest>
 {
     public ICommimentsDbContext _context { get; set; }

     public RemoveBehaviourTypeCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task Handle(RemoveBehaviourTypeRequest request, CancellationToken cancellationToken)
     {
         _context.BehaviourTypes.Remove(await _context.BehaviourTypes.FindAsync(request.BehaviourTypeId));
         await _context.SaveChangesAsync(cancellationToken);
     }

 }

