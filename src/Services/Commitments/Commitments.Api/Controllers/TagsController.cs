// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel.TagAggregate.Commands;
using Commitments.Core.AggregateModel.TagAggregate.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace Commitments.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/tags")]
public class TagsController
{
    private readonly IMediator _mediator;

    public TagsController(IMediator mediator) => _mediator = mediator;

    [HttpGet("slug/{slug}")]
    public async Task<ActionResult<GetTagBySlugResponse>> GetBySlug([FromRoute] GetTagBySlugRequest request)
        => await _mediator.Send(request);

    [HttpPost]
    public async Task<ActionResult<SaveTagResponse>> Add(SaveTagRequest request)
        => await _mediator.Send(request);

    [HttpDelete("{tagId}")]
    public async Task Remove([FromRoute] RemoveTagRequest request)
        => await _mediator.Send(request);

    [HttpGet("{tagId}")]
    public async Task<ActionResult<GetTagByIdResponse>> GetById([FromRoute] GetTagByIdRequest request)
        => await _mediator.Send(request);

    [HttpGet]
    public async Task<ActionResult<GetTagsResponse>> Get()
        => await _mediator.Send(new GetTagsRequest());
}

