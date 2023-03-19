// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using DashboardService.Core.AggregateModel.CardLayoutAggregate.Commands;
using DashboardService.Core.AggregateModel.CardLayoutAggregate.Queries;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Swashbuckle.AspNetCore.Annotations;

namespace DashboardService.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/{version:apiVersion}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class CardLayoutController
{
    private readonly IMediator _mediator;

    private readonly ILogger<CardLayoutController> _logger;

    public CardLayoutController(IMediator mediator, ILogger<CardLayoutController> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [SwaggerOperation(
        Summary = "Update CardLayout",
        Description = @"Update CardLayout"
    )]
    [HttpPut(Name = "updateCardLayout")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UpdateCardLayoutResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<UpdateCardLayoutResponse>> Update([FromBody] UpdateCardLayoutRequest request, CancellationToken cancellationToken)
    {
        return await _mediator.Send(request, cancellationToken);
    }

    [SwaggerOperation(
        Summary = "Create CardLayout",
        Description = @"Create CardLayout"
    )]
    [HttpPost(Name = "createCardLayout")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(CreateCardLayoutResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<CreateCardLayoutResponse>> Create([FromBody] CreateCardLayoutRequest request, CancellationToken cancellationToken)
    {
        return await _mediator.Send(request, cancellationToken);
    }

    [SwaggerOperation(
        Summary = "Get CardLayouts",
        Description = @"Get CardLayouts"
    )]
    [HttpGet(Name = "getCardLayouts")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetCardLayoutsResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetCardLayoutsResponse>> Get(CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetCardLayoutsRequest(), cancellationToken);
    }

    [SwaggerOperation(
        Summary = "Get CardLayout by id",
        Description = @"Get CardLayout by id"
    )]
    [HttpGet("{cardLayoutId:guid}", Name = "getCardLayoutById")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetCardLayoutByIdResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetCardLayoutByIdResponse>> GetById([FromRoute] Guid cardLayoutId, CancellationToken cancellationToken)
    {
        var request = new GetCardLayoutByIdRequest() { CardLayoutId = cardLayoutId };

        var response = await _mediator.Send(request, cancellationToken);

        if (response.CardLayout == null)
        {
            return new NotFoundObjectResult(request.CardLayoutId);
        }

        return response;
    }

    [SwaggerOperation(
        Summary = "Delete CardLayout",
        Description = @"Delete CardLayout"
    )]
    [HttpDelete("{cardLayoutId:guid}", Name = "deleteCardLayout")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(DeleteCardLayoutResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<DeleteCardLayoutResponse>> Delete([FromRoute] Guid cardLayoutId, CancellationToken cancellationToken)
    {
        var request = new DeleteCardLayoutRequest() { CardLayoutId = cardLayoutId };

        return await _mediator.Send(request, cancellationToken);
    }

}


