// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Commitments.Api.Features.Achievements;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AchievementsController
{
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AchievementsController(IHttpContextAccessor httpContextAccessor, IMediator mediator) {
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public async Task<ActionResult<GetAchievementsResponse>> Get() {
        return await _mediator.Send(new GetAchievementsRequest() {
            ProfileId = _httpContextAccessor.GetProfileIdFromClaims()
        });
    }
}

