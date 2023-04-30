// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using IdentityService.Core.AggregateModel.PrivilegeAggregate.Commands;
using IdentityService.Core.AggregateModel.PrivilegeAggregate.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Net.Mime;

namespace IdentityService.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/{version:apiVersion}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class PrivilegeController
{
    private readonly ISender _sender;

    private readonly ILogger<PrivilegeController> _logger;

    public PrivilegeController(ISender sender, ILogger<PrivilegeController> logger)
    {
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [SwaggerOperation(
        Summary = "Update PrivilegeId",
        Description = @"Update PrivilegeId"
    )]
    [HttpPut(Name = "updatePrivilegeId")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UpdatePrivilegeResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<UpdatePrivilegeResponse>> Update([FromBody] UpdatePrivilegeRequest request, CancellationToken cancellationToken)
    {
        return await _sender.Send(request, cancellationToken);
    }

    [SwaggerOperation(
        Summary = "Create Privilege",
        Description = @"Create Privilege"
    )]
    [HttpPost(Name = "createPrivilege")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(CreatePrivilegeResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<CreatePrivilegeResponse>> Create([FromBody] CreatePrivilegeRequest request, CancellationToken cancellationToken)
    {
        return await _sender.Send(request, cancellationToken);
    }

    [SwaggerOperation(
        Summary = "Get Privileges",
        Description = @"Get Privileges"
    )]
    [HttpGet(Name = "getPrivileges")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetPrivilegesResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetPrivilegesResponse>> Get(CancellationToken cancellationToken)
    {
        return await _sender.Send(new GetPrivilegesRequest(), cancellationToken);
    }

    [SwaggerOperation(
        Summary = "Get PrivilegeId  by id",
        Description = @"Get PrivilegeId by id"
    )]
    [HttpGet("{toDoId:guid}", Name = "getPrivilegeIdById")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetPrivilegeByIdResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetPrivilegeByIdResponse>> GetById([FromRoute] Guid privilegeId, CancellationToken cancellationToken)
    {
        var request = new GetPrivilegeByIdRequest() { PrivilegeId = privilegeId };

        var response = await _sender.Send(request, cancellationToken);

        if (response.Privilege == null)
        {
            return new NotFoundObjectResult(request.PrivilegeId);
        }

        return response;
    }

    [SwaggerOperation(
        Summary = "Delete Privilege",
        Description = @"Delete Privilege"
    )]
    [HttpDelete("{toDoId:guid}", Name = "deletePrivilege")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(DeletePrivilegeResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<DeletePrivilegeResponse>> Delete([FromRoute] Guid privilegeId, CancellationToken cancellationToken)
    {
        var request = new DeletePrivilegeRequest() { PrivilegeId = privilegeId };

        return await _sender.Send(request, cancellationToken);
    }

}