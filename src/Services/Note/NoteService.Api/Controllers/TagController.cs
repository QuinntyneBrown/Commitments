// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using NoteService.Core.AggregateModel.TagAggregate.Commands;
using NoteService.Core.AggregateModel.TagAggregate.Queries;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Swashbuckle.AspNetCore.Annotations;

namespace NoteService.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/{version:apiVersion}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class TagController
{
    private readonly ISender _sender;

    private readonly ILogger<TagController> _logger;

    public TagController(ISender sender, ILogger<TagController> logger)
    {
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [SwaggerOperation(
        Summary = "Update Tag",
        Description = @"Update Tag"
    )]
    [HttpPut(Name = "updateTag")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UpdateTagResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<UpdateTagResponse>> Update([FromBody] UpdateTagRequest request, CancellationToken cancellationToken)
    {
        return await _sender.Send(request, cancellationToken);
    }

    [SwaggerOperation(
        Summary = "Create Tag",
        Description = @"Create Tag"
    )]
    [HttpPost(Name = "createTag")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(CreateTagResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<CreateTagResponse>> Create([FromBody] CreateTagRequest request, CancellationToken cancellationToken)
    {
        return await _sender.Send(request, cancellationToken);
    }

    [SwaggerOperation(
        Summary = "Get Tags",
        Description = @"Get Tags"
    )]
    [HttpGet(Name = "getTags")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetTagsResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetTagsResponse>> Get(CancellationToken cancellationToken)
    {
        return await _sender.Send(new GetTagsRequest(), cancellationToken);
    }

    [SwaggerOperation(
        Summary = "Get Tag by id",
        Description = @"Get Tag by id"
    )]
    [HttpGet("{tagId:guid}", Name = "getTagById")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetTagByIdResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetTagByIdResponse>> GetById([FromRoute] Guid tagId, CancellationToken cancellationToken)
    {
        var request = new GetTagByIdRequest() { TagId = tagId };

        var response = await _sender.Send(request, cancellationToken);

        if (response.Tag == null)
        {
            return new NotFoundObjectResult(request.TagId);
        }

        return response;
    }

    [SwaggerOperation(
        Summary = "Delete Tag",
        Description = @"Delete Tag"
    )]
    [HttpDelete("{tagId:guid}", Name = "deleteTag")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(DeleteTagResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<DeleteTagResponse>> Delete([FromRoute] Guid tagId, CancellationToken cancellationToken)
    {
        var request = new DeleteTagRequest() { TagId = tagId };

        return await _sender.Send(request, cancellationToken);
    }

}