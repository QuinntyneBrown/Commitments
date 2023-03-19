// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using ToDoService.Core.AggregateModel.ToDoAggregate.Commands;
using ToDoService.Core.AggregateModel.ToDoAggregate.Queries;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Swashbuckle.AspNetCore.Annotations;

namespace ToDoService.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/{version:apiVersion}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class ToDoController
{
    private readonly IMediator _mediator;

    private readonly ILogger<ToDoController> _logger;

    public ToDoController(IMediator mediator, ILogger<ToDoController> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [SwaggerOperation(
        Summary = "Update ToDo",
        Description = @"Update ToDo"
    )]
    [HttpPut(Name = "updateToDo")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UpdateToDoResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<UpdateToDoResponse>> Update([FromBody] UpdateToDoRequest request, CancellationToken cancellationToken)
    {
        return await _mediator.Send(request, cancellationToken);
    }

    [SwaggerOperation(
        Summary = "Create ToDo",
        Description = @"Create ToDo"
    )]
    [HttpPost(Name = "createToDo")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(CreateToDoResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<CreateToDoResponse>> Create([FromBody] CreateToDoRequest request, CancellationToken cancellationToken)
    {
        return await _mediator.Send(request, cancellationToken);
    }

    [SwaggerOperation(
        Summary = "Get ToDos",
        Description = @"Get ToDos"
    )]
    [HttpGet(Name = "getToDos")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetToDosResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetToDosResponse>> Get(CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetToDosRequest(), cancellationToken);
    }

    [SwaggerOperation(
        Summary = "Get ToDo by id",
        Description = @"Get ToDo by id"
    )]
    [HttpGet("{toDoId:guid}", Name = "getToDoById")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetToDoByIdResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetToDoByIdResponse>> GetById([FromRoute] Guid toDoId, CancellationToken cancellationToken)
    {
        var request = new GetToDoByIdRequest() { ToDoId = toDoId };

        var response = await _mediator.Send(request, cancellationToken);

        if (response.ToDo == null)
        {
            return new NotFoundObjectResult(request.ToDoId);
        }

        return response;
    }

    [SwaggerOperation(
        Summary = "Delete ToDo",
        Description = @"Delete ToDo"
    )]
    [HttpDelete("{toDoId:guid}", Name = "deleteToDo")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(DeleteToDoResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<DeleteToDoResponse>> Delete([FromRoute] Guid toDoId, CancellationToken cancellationToken)
    {
        var request = new DeleteToDoRequest() { ToDoId = toDoId };

        return await _mediator.Send(request, cancellationToken);
    }

}


