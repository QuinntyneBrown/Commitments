// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace ToDoService.Core.AggregateModel.ToDoAggregate.Commands;

public class UpdateToDoRequestValidator: AbstractValidator<UpdateToDoRequest>
{
    public UpdateToDoRequestValidator(){

        RuleFor(x => x.ToDoId).NotEqual(default(Guid));
        RuleFor(x => x.Name).NotNull();
        RuleFor(x => x.Description).NotNull();
        RuleFor(x => x.ProfileId).NotEqual(default(Guid));
        RuleFor(x => x.Due).NotNull();
        RuleFor(x => x.CompletedOn).NotNull();

    }

}


public class UpdateToDoRequest: IRequest<UpdateToDoResponse>
{
    public Guid ToDoId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid ProfileId { get; set; }
    public DateTime Due { get; set; }
    public DateTime CompletedOn { get; set; }
}


public class UpdateToDoResponse
{
    public required ToDoDto ToDo { get; set; }
}


public class UpdateToDoRequestHandler: IRequestHandler<UpdateToDoRequest,UpdateToDoResponse>
{
    private readonly IToDoServiceDbContext _context;

    private readonly ILogger<UpdateToDoRequestHandler> _logger;

    public UpdateToDoRequestHandler(ILogger<UpdateToDoRequestHandler> logger,IToDoServiceDbContext context){
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<UpdateToDoResponse> Handle(UpdateToDoRequest request,CancellationToken cancellationToken)
    {
        var toDo = await _context.ToDos.SingleAsync(x => x.ToDoId == request.ToDoId);

        toDo.ToDoId = request.ToDoId;
        toDo.Name = request.Name;
        toDo.Description = request.Description;
        toDo.ProfileId = request.ProfileId;
        toDo.Due = request.Due;
        toDo.CompletedOn = request.CompletedOn;

        await _context.SaveChangesAsync(cancellationToken);

        return new ()
        {
            ToDo = toDo.ToDto()
        };

    }

}



