using Asp.Versioning;
using Identity.Features.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Net.Mime;

namespace Identity.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/{version:apiVersion}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class UserController
{
    private readonly ISender _sender;
    private readonly ILogger<UserController> _logger;

    public UserController(ISender sender, ILogger<UserController> logger)
    {
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [SwaggerOperation(Summary = "Create User", Description = "Create User")]
    [HttpPost(Name = "createUser")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(CreateUserResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<CreateUserResponse>> Create([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        return await _sender.Send(request, cancellationToken);
    }

    [SwaggerOperation(Summary = "Update User", Description = "Update User")]
    [HttpPut(Name = "updateUser")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UpdateUserResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<UpdateUserResponse>> Update([FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        return await _sender.Send(request, cancellationToken);
    }

    [SwaggerOperation(Summary = "Get Users", Description = "Get Users")]
    [HttpGet(Name = "getUsers")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetUsersResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetUsersResponse>> Get(CancellationToken cancellationToken)
    {
        return await _sender.Send(new GetUsersRequest(), cancellationToken);
    }

    [SwaggerOperation(Summary = "Get User by id", Description = "Get User by id")]
    [HttpGet("{userId:guid}", Name = "getUserById")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetUserByIdResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetUserByIdResponse>> GetById([FromRoute] Guid userId, CancellationToken cancellationToken)
    {
        var request = new GetUserByIdRequest() { UserId = userId };
        var response = await _sender.Send(request, cancellationToken);
        if (response.User == null) return new NotFoundObjectResult(request.UserId);
        return response;
    }

    [SwaggerOperation(Summary = "Delete User", Description = "Delete User")]
    [HttpDelete("{userId:guid}", Name = "deleteUser")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(DeleteUserResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<DeleteUserResponse>> Delete([FromRoute] Guid userId, CancellationToken cancellationToken)
    {
        return await _sender.Send(new DeleteUserRequest() { UserId = userId }, cancellationToken);
    }
}
