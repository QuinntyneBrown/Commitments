// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.AggregateModel;
using Commitments.Core.Interfaces;


namespace Commitments.Core.AggregateModel.ToDoAggregate.Commands;

public class SaveToDoCommandValidator : AbstractValidator<SaveToDoRequest>
{
    public SaveToDoCommandValidator()
    {
        RuleFor(request => request.ToDo.ToDoId).NotNull();
    }
}

public class SaveToDoRequest : IRequest<SaveToDoResponse>
{
    public ToDoDto ToDo { get; set; }
}

public class SaveToDoResponse
{
    public Guid ToDoId { get; set; }
}

public class SaveToDoCommandHandler : IRequestHandler<SaveToDoRequest, SaveToDoResponse>
{
    public ICommimentsDbContext _context { get; set; }


    public async Task<SaveToDoResponse> Handle(SaveToDoRequest request, CancellationToken cancellationToken)
    {
        var toDo = await _context.ToDos.FindAsync(request.ToDo.ToDoId);

        if (toDo == null) _context.ToDos.Add(toDo = new ToDo());

        toDo.Name = request.ToDo.Name;
        toDo.CompletedOn = request.ToDo.CompletedOn;
        toDo.DueOn = request.ToDo.DueOn;
        toDo.Description = request.ToDo.Description;
        toDo.ProfileId = request.ToDo.ProfileId;

        await _context.SaveChangesAsync(cancellationToken);

        return new SaveToDoResponse() { ToDoId = toDo.ToDoId };
    }
}

