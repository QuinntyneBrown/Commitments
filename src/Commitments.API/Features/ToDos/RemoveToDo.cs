// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.AggregateModel;
using Commitments.Core.Interfaces;


namespace Commitments.Api.Features.ToDos;

 public class RemoveToDoCommandValidator : AbstractValidator<RemoveToDoRequest>
 {
     public RemoveToDoCommandValidator()
     {
         RuleFor(request => request.ToDoId).NotEqual(0);
     }
 }

 public class RemoveToDoRequest : IRequest
 {
     public int ToDoId { get; set; }
 }

 public class RemoveToDoCommandHandler : IRequestHandler<RemoveToDoRequest>
 {
     public ICommimentsDbContext _context { get; set; }


     public async Task Handle(RemoveToDoRequest request, CancellationToken cancellationToken)
     {
         _context.ToDos.Remove(await _context.ToDos.FindAsync(request.ToDoId));
         await _context.SaveChangesAsync(cancellationToken);
     }

 }

