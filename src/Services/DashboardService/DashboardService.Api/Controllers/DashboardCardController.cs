// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using DashboardService.Core.AggregateModel.DashboardCardAggregate.Commands;
using DashboardService.Core.AggregateModel.DashboardCardAggregate.Queries;
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
public class DashboardCardController
{
    private readonly IMediator _mediator;

    private readonly ILogger<DashboardCardController> _logger;

    public DashboardCardController(IMediator mediator,ILogger<DashboardCardController> logger){
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [SwaggerOperation(
        Summary = "Update DashboardCard",
        Description = @"Update DashboardCard"
    )]
    [HttpPut(Name = "updateDashboardCard")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UpdateDashboardCardResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<UpdateDashboardCardResponse>> Update([FromBody]UpdateDashboardCardRequest  request,CancellationToken cancellationToken)
    {
        return await _mediator.Send(request, cancellationToken);
    }

    [SwaggerOperation(
        Summary = "Create DashboardCard",
        Description = @"Create DashboardCard"
    )]
    [HttpPost(Name = "createDashboardCard")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(CreateDashboardCardResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<CreateDashboardCardResponse>> Create([FromBody]CreateDashboardCardRequest  request,CancellationToken cancellationToken)
    {
        return await _mediator.Send(request, cancellationToken);
    }

    [SwaggerOperation(
        Summary = "Get DashboardCards",
        Description = @"Get DashboardCards"
    )]
    [HttpGet(Name = "getDashboardCards")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetDashboardCardsResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetDashboardCardsResponse>> Get(CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetDashboardCardsRequest(), cancellationToken);
    }

    [SwaggerOperation(
        Summary = "Get Range",
        Description = @"Get Range"
    )]
    [HttpGet("range")]
    public async Task<ActionResult<GetDashboardCardByIdsResponse>> GetByIds([FromQuery] GetDashboardCardByIdsRequest request)
        => await _mediator.Send(request);

    [SwaggerOperation(
        Summary = "Create Range",
        Description = @"Create Range"
    )]
    [HttpPost("range")]
    public async Task<ActionResult<CreateDashboardCardRangeResponse>> SaveRange(CreateDashboardCardRangeRequest request)
        => await _mediator.Send(request);

    [SwaggerOperation(
        Summary = "Get DashboardCard by id",
        Description = @"Get DashboardCard by id"
    )]
    [HttpGet("{dashboardCardId:guid}", Name = "getDashboardCardById")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetDashboardCardByIdResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetDashboardCardByIdResponse>> GetById([FromRoute]Guid dashboardCardId,CancellationToken cancellationToken)
    {
        var request = new GetDashboardCardByIdRequest(){DashboardCardId = dashboardCardId};

        var response = await _mediator.Send(request, cancellationToken);

        if (response.DashboardCard == null)
        {
            return new NotFoundObjectResult(request.DashboardCardId);
        }

        return response;
    }

    [SwaggerOperation(
        Summary = "Delete DashboardCard",
        Description = @"Delete DashboardCard"
    )]
    [HttpDelete("{dashboardCardId:guid}", Name = "deleteDashboardCard")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(DeleteDashboardCardResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<DeleteDashboardCardResponse>> Delete([FromRoute]Guid dashboardCardId,CancellationToken cancellationToken)
    {
        var request = new DeleteDashboardCardRequest() {DashboardCardId = dashboardCardId };

        return await _mediator.Send(request, cancellationToken);
    }

}


