using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Api.Features.ToDos;

 public class GetToDoByIdValidator : AbstractValidator<GetToDoByIdRequest>
 {
     public GetToDoByIdValidator()
     {
         RuleFor(request => request.ToDoId).NotEqual(0);
     }
 }

 public class GetToDoByIdRequest : IRequest<GetToDoByIdResponse> {
     public int ToDoId { get; set; }
 }

 public class GetToDoByIdResponse
 {
     public ToDoDto ToDo { get; set; }
 }

 public class GetToDoByIdHandler : IRequestHandler<GetToDoByIdRequest, GetToDoByIdResponse>
 {
     public ICommimentsDbContext _context { get; set; }


     public async Task<GetToDoByIdResponse> Handle(GetToDoByIdRequest request, CancellationToken cancellationToken)
         => new GetToDoByIdResponse()
         {
             ToDo = ToDoDto.FromToDo(await _context.ToDos.FindAsync(request.ToDoId))
         };
 }
