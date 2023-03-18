// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Core.AggregateModel.ToDoAggregate.Queries;

public class GetToDoByIdValidator : AbstractValidator<GetToDoByIdRequest>
{
    public GetToDoByIdValidator()
    {
        RuleFor(request => request.ToDoId).NotEqual(default(Guid));
    }
}

public class GetToDoByIdRequest : IRequest<GetToDoByIdResponse>
{
    public Guid ToDoId { get; set; }
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

