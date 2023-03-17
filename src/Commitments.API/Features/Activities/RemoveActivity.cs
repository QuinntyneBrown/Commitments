// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.AggregateModel;
using Commitments.Core.Interfaces;


namespace Commitments.Api.Features.Activities;

 public class RemoveActivityCommandValidator : AbstractValidator<RemoveActivityRequest>
 {
     public RemoveActivityCommandValidator()
     {
         RuleFor(request => request.ActivityId).NotEqual(0);
     }
 }

 public class RemoveActivityRequest : IRequest
 {
     public int ActivityId { get; set; }
 }

 public class RemoveActivityCommandHandler : IRequestHandler<RemoveActivityRequest>
 {
     public ICommimentsDbContext _context { get; set; }

     public RemoveActivityCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task Handle(RemoveActivityRequest request, CancellationToken cancellationToken)
     {
         _context.Activities.Remove(await _context.Activities.FindAsync(request.ActivityId));
         await _context.SaveChangesAsync(cancellationToken);
     }
 }

