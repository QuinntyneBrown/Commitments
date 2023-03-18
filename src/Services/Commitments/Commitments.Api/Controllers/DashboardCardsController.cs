// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel.DashboardCardAggregate.Commands;
using Commitments.Core.AggregateModel.DashboardCardAggregate.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace Commitments.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/dashboardCards")]
public class DashboardCardsController
{
    private readonly IMediator _mediator;

    public DashboardCardsController(IMediator mediator) => _mediator = mediator;

    [HttpPost("range")]
    public async Task<ActionResult<SaveDashboardCardRangeResponse>> SaveRange(SaveDashboardCardRangeRequest request)
        => await _mediator.Send(request);

    [HttpGet("range")]
    public async Task<ActionResult<GetDashboardCardByIdsResponse>> GetByIds([FromQuery] GetDashboardCardByIdsRequest request)
        => await _mediator.Send(request);

    [HttpPost]
    public async Task<ActionResult<SaveDashboardCardResponse>> Save(SaveDashboardCardRequest request)
        => await _mediator.Send(request);

    [HttpDelete("{dashboardCardId}")]
    public async Task Remove([FromRoute] RemoveDashboardCardRequest request)
        => await _mediator.Send(request);

    [HttpGet("{dashboardCardId}")]
    public async Task<ActionResult<GetDashboardCardByIdResponse>> GetById([FromRoute] GetDashboardCardByIdRequest request)
        => await _mediator.Send(request);


    [HttpGet]
    public async Task<ActionResult<GetDashboardCardsResponse>> Get()
        => await _mediator.Send(new GetDashboardCardsRequest());
}

