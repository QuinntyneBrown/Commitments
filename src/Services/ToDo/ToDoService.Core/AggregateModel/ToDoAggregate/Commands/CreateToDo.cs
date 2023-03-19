// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace ToDoService.Core.AggregateModel.ToDoAggregate.Commands;

public class CreateToDoRequestValidator : AbstractValidator<CreateToDoRequest>
{
    public CreateToDoRequestValidator()
    {

        RuleFor(x => x.Name).NotNull();
        RuleFor(x => x.Description).NotNull();
        RuleFor(x => x.ProfileId).NotEqual(default(Guid));
        RuleFor(x => x.Due).NotNull();
        RuleFor(x => x.CompletedOn).NotNull();
    }
}

public class CreateToDoRequest : IRequest<CreateToDoResponse>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid ProfileId { get; set; }
    public DateTime Due { get; set; }
    public DateTime CompletedOn { get; set; }
}

public class CreateToDoResponse
{
    public required ToDoDto ToDo { get; set; }
}

public class CreateToDoRequestHandler : IRequestHandler<CreateToDoRequest, CreateToDoResponse>
{
    private readonly IToDoServiceDbContext _context;

    private readonly ILogger<CreateToDoRequestHandler> _logger;

    public CreateToDoRequestHandler(ILogger<CreateToDoRequestHandler> logger, IToDoServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<CreateToDoResponse> Handle(CreateToDoRequest request, CancellationToken cancellationToken)
    {
        var toDo = new ToDo();

        _context.ToDos.Add(toDo);

        toDo.Name = request.Name;
        toDo.Description = request.Description;
        toDo.ProfileId = request.ProfileId;
        toDo.Due = request.Due;
        toDo.CompletedOn = request.CompletedOn;

        await _context.SaveChangesAsync(cancellationToken);

        return new()
        {
            ToDo = toDo.ToDto()
        };
    }
}