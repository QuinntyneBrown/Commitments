using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Api.Features.ToDos;

 public class GetToDoByIdQueryCommandValidator : AbstractValidator<GetToDoByIdQueryRequest>
 {
     public GetToDoByIdQueryCommandValidator()
     {
         RuleFor(request => request.ToDoId).NotEqual(0);
     }
 }

 public class GetToDoByIdQueryRequest : IRequest<GetToDoByIdQueryResponse> {
     public int ToDoId { get; set; }
 }

 public class GetToDoByIdQueryResponse
 {
     public ToDoDto ToDo { get; set; }
 }

 public class GetToDoByIdQueryCommandHandler : IRequestHandler<GetToDoByIdQueryRequest, GetToDoByIdQueryResponse>
 {
     public IAppDbContext _context { get; set; }


     public async Task<GetToDoByIdQueryResponse> Handle(GetToDoByIdQueryRequest request, CancellationToken cancellationToken)
         => new GetToDoByIdQueryResponse()
         {
             ToDo = ToDoDto.FromToDo(await _context.ToDos.FindAsync(request.ToDoId))
         };
 }
