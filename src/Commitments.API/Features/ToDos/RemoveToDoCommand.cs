using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;


namespace Commitments.Api.Features.ToDos;

 public class RemoveToDoCommandValidator : AbstractValidator<RemoveToDoCommandRequest>
 {
     public RemoveToDoCommandValidator()
     {
         RuleFor(request => request.ToDoId).NotEqual(0);
     }
 }

 public class RemoveToDoCommandRequest : IRequest
 {
     public int ToDoId { get; set; }
 }

 public class RemoveToDoCommandHandler : IRequestHandler<RemoveToDoCommandRequest>
 {
     public IAppDbContext _context { get; set; }


     public async Task Handle(RemoveToDoCommandRequest request, CancellationToken cancellationToken)
     {
         _context.ToDos.Remove(await _context.ToDos.FindAsync(request.ToDoId));
         await _context.SaveChangesAsync(cancellationToken);
     }

 }
