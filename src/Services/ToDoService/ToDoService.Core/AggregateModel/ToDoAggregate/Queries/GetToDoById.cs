// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace ToDoService.Core.AggregateModel.ToDoAggregate.Queries;

public class GetToDoByIdRequest: IRequest<GetToDoByIdResponse>
{
    public Guid ToDoId { get; set; }
}


public class GetToDoByIdResponse
{
    public required ToDoDto ToDo { get; set; }
}


public class GetToDoByIdRequestHandler: IRequestHandler<GetToDoByIdRequest,GetToDoByIdResponse>
{
    private readonly IToDoServiceDbContext _context;

    private readonly ILogger<GetToDoByIdRequestHandler> _logger;

    public GetToDoByIdRequestHandler(ILogger<GetToDoByIdRequestHandler> logger,IToDoServiceDbContext context){
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetToDoByIdResponse> Handle(GetToDoByIdRequest request,CancellationToken cancellationToken)
    {
        return new () {
            ToDo = (await _context.ToDos.AsNoTracking().SingleOrDefaultAsync(x => x.ToDoId == request.ToDoId)).ToDto()
        };

    }

}



