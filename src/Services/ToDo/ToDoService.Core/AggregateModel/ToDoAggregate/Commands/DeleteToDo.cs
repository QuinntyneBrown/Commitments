// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace ToDoService.Core.AggregateModel.ToDoAggregate.Commands;

public class DeleteToDoRequestValidator: AbstractValidator<DeleteToDoRequest>
{
    public DeleteToDoRequestValidator(){
        RuleFor(x => x.ToDoId).NotEqual(default(Guid));
    }
}

public class DeleteToDoRequest: IRequest<DeleteToDoResponse>
{
    public Guid ToDoId { get; set; }
}


public class DeleteToDoResponse
{
    public required ToDoDto ToDo { get; set; }
}


public class DeleteToDoRequestHandler: IRequestHandler<DeleteToDoRequest,DeleteToDoResponse>
{
    private readonly IToDoServiceDbContext _context;

    private readonly ILogger<DeleteToDoRequestHandler> _logger;

    public DeleteToDoRequestHandler(ILogger<DeleteToDoRequestHandler> logger,IToDoServiceDbContext context){
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<DeleteToDoResponse> Handle(DeleteToDoRequest request,CancellationToken cancellationToken)
    {
        var toDo = await _context.ToDos.FindAsync(request.ToDoId);

        _context.ToDos.Remove(toDo);

        await _context.SaveChangesAsync(cancellationToken);

        return new ()
        {
            ToDo = toDo.ToDto()
        };
    }
}