// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Features.GoalProgress;
using Commitments.Features.Relations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Commitments.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/relations")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class RelationsController
{
    private const int DefaultTop = 3;
    private readonly ISender _sender;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RelationsController(IHttpContextAccessor httpContextAccessor, ISender sender)
    {
        _sender = sender;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet("summary")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetRelationsSummaryResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetRelationsSummaryResponse>> GetSummary(
        [FromQuery] DateTimeOffset? asOf,
        [FromQuery] int? top)
    {
        var response = _httpContextAccessor.HttpContext?.Response;
        if (response is not null)
            response.Headers.CacheControl = CacheControlPolicy.For(asOf, DateTimeOffset.UtcNow);

        return await _sender.Send(new GetRelationsSummaryRequest
        {
            ProfileId = _httpContextAccessor.GetProfileId(),
            AsOf = asOf,
            Top = top ?? DefaultTop
        });
    }
}
