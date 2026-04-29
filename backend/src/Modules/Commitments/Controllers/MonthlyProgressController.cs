// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Features.GoalProgress;
using Commitments.Features.MonthlyProgress;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Commitments.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/monthly-progress")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class MonthlyProgressController
{
    private readonly ISender _sender;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MonthlyProgressController(IHttpContextAccessor httpContextAccessor, ISender sender)
    {
        _sender = sender;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetMonthlyProgressResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetMonthlyProgressResponse>> Get([FromQuery] DateTimeOffset? asOf)
    {
        var response = _httpContextAccessor.HttpContext?.Response;
        if (response is not null)
            response.Headers.CacheControl = CacheControlPolicy.For(asOf, DateTimeOffset.UtcNow);

        return await _sender.Send(new GetMonthlyProgressRequest
        {
            ProfileId = _httpContextAccessor.GetProfileId(),
            AsOf = asOf
        });
    }
}
