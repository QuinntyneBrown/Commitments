// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel.CommitmentAggregate.Commands;
using Commitments.Core.AggregateModel.CommitmentAggregate.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Commitments.Api.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/{version:apiVersion}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class CommitmentController
{
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CommitmentController(IHttpContextAccessor httpContextAccessor, IMediator mediator)
    {
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(SaveCommitmentResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<SaveCommitmentResponse>> Save(SaveCommitmentRequest request)
    {
        request.Commitment.ProfileId = _httpContextAccessor.GetProfileId();
        return await _mediator.Send(request);
    }

    [HttpDelete("{commitmentId}")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task Remove(RemoveCommitmentRequest request)
        => await _mediator.Send(request);

    [HttpGet("{CommitmentId}")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetCommitmentByIdResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetCommitmentByIdResponse>> GetById([FromRoute] GetCommitmentByIdRequest request)
        => await _mediator.Send(request);

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetCommitmentsResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetCommitmentsResponse>> Get()
        => await _mediator.Send(new GetCommitmentsRequest());

    [HttpGet("personal")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetPersonalCommitmentsResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetPersonalCommitmentsResponse>> GetPersonal()
        => await _mediator.Send(new GetPersonalCommitmentsRequest() { ProfileId = _httpContextAccessor.GetProfileId() });

    [HttpGet("daily")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetDailyCommitmentsResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetDailyCommitmentsResponse>> GetDaily()
        => await _mediator.Send(new GetDailyCommitmentsRequest() { ProfileId = _httpContextAccessor.GetProfileId() });
}

