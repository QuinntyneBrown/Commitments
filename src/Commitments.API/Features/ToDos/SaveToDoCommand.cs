using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;


namespace Commitments.Api.Features.ToDos;

 public class SaveToDoCommandValidator: AbstractValidator<SaveToDoCommandRequest> {
     public SaveToDoCommandValidator()
     {
         RuleFor(request => request.ToDo.ToDoId).NotNull();
     }
 }

 public class SaveToDoCommandRequest : IRequest<SaveToDoCommandResponse> {
     public ToDoApiModel ToDo { get; set; }
 }

 public class SaveToDoCommandResponse
 {			
     public int ToDoId { get; set; }
 }

 public class SaveToDoCommandHandler : IRequestHandler<SaveToDoCommandRequest, SaveToDoCommandResponse>
 {
     public IAppDbContext _context { get; set; }


     public async Task<SaveToDoCommandResponse> Handle(SaveToDoCommandRequest request, CancellationToken cancellationToken)
     {
         var toDo = await _context.ToDos.FindAsync(request.ToDo.ToDoId);

         if (toDo == null) _context.ToDos.Add(toDo = new ToDo());

         toDo.Name = request.ToDo.Name;
         toDo.CompletedOn = request.ToDo.CompletedOn;
         toDo.DueOn = request.ToDo.DueOn;
         toDo.Description = request.ToDo.Description;
         toDo.ProfileId = request.ToDo.ProfileId;

         await _context.SaveChangesAsync(cancellationToken);

         return new SaveToDoCommandResponse() { ToDoId = toDo.ToDoId };
     }
 }
