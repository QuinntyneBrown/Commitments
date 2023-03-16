using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Api.Features.ToDos;

 public class GetToDoByIdQueryCommandValidator : AbstractValidator<GetToDoByIdQueryCommandRequest>
 {
     public GetToDoByIdQueryCommandValidator()
     {
         RuleFor(request => request.ToDoId).NotEqual(0);
     }
 }

 public class GetToDoByIdQueryCommandRequest : IRequest<GetToDoByIdQueryCommandResponse> {
     public int ToDoId { get; set; }
 }

 public class GetToDoByIdQueryCommandResponse
 {
     public ToDoApiModel ToDo { get; set; }
 }

 public class GetToDoByIdQueryCommandHandler : IRequestHandler<GetToDoByIdQueryCommandRequest, GetToDoByIdQueryCommandResponse>
 {
     public IAppDbContext _context { get; set; }


     public async Task<GetToDoByIdQueryCommandResponse> Handle(GetToDoByIdQueryCommandRequest request, CancellationToken cancellationToken)
         => new GetToDoByIdQueryCommandResponse()
         {
             ToDo = ToDoApiModel.FromToDo(await _context.ToDos.FindAsync(request.ToDoId))
         };
 }
