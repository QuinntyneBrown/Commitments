// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace Commitments.Api.Features.Commitments;

[Authorize]
[ApiController]
[Route("api/commitments")]
public class CommitmentsController
{
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CommitmentsController(IHttpContextAccessor httpContextAccessor, IMediator mediator)
    {
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    public async Task<ActionResult<SaveCommitmentResponse>> Save(SaveCommitmentRequest request) {
        request.Commitment.ProfileId = _httpContextAccessor.GetProfileIdFromClaims();
        return await _mediator.Send(request);
    }

    [HttpDelete("{commitmentId}")]
    public async Task Remove(RemoveCommitmentRequest request)
        => await _mediator.Send(request);            

    [HttpGet("{CommitmentId}")]
    public async Task<ActionResult<GetCommitmentByIdResponse>> GetById([FromRoute]GetCommitmentByIdRequest request)
        => await _mediator.Send(request);

    [HttpGet]
    public async Task<ActionResult<GetCommitmentsResponse>> Get()
        => await _mediator.Send(new GetCommitmentsRequest());

    [HttpGet("personal")]
    public async Task<ActionResult<GetPersonalCommitmentsResponse>> GetPersonal()
        => await _mediator.Send(new GetPersonalCommitmentsRequest() { ProfileId = _httpContextAccessor.GetProfileIdFromClaims() });

    [HttpGet("daily")]
    public async Task<ActionResult<GetDailyCommitmentsResponse>> GetDaily() 
        => await _mediator.Send(new GetDailyCommitmentsRequest() { ProfileId = _httpContextAccessor.GetProfileIdFromClaims() });
}

