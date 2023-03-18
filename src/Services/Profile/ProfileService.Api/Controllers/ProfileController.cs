// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Kernel;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Core.AggregateModel.ProfileAggregate.Commands;
using ProfileService.Core.AggregateModel.ProfileAggregate.Queries;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Net.Mime;

namespace ProfileService.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/{version:apiVersion}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class ProfileController
{
    private readonly IMediator _mediator;

    private readonly ILogger<ProfileController> _logger;

    public ProfileController(IMediator mediator, ILogger<ProfileController> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [SwaggerOperation(
        Summary = "Update Profile",
        Description = @"Update Profile"
    )]
    [HttpPut(Name = "updateProfile")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UpdateProfileResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<UpdateProfileResponse>> Update([FromBody] UpdateProfileRequest request, CancellationToken cancellationToken)
    {
        return await _mediator.Send(request, cancellationToken);
    }

    [SwaggerOperation(
        Summary = "Create Profile",
        Description = @"Create Profile"
    )]
    [HttpPost(Name = "createProfile")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(CreateProfileResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<CreateProfileResponse>> Create([FromBody] CreateProfileRequest request, CancellationToken cancellationToken)
    {
        return await _mediator.Send(request, cancellationToken);
    }

    [SwaggerOperation(
        Summary = "Get Profiles",
        Description = @"Get Profiles"
    )]
    [HttpGet(Name = "getProfiles")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetProfilesResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetProfilesResponse>> Get(CancellationToken cancellationToken)
    {
        throw new DomainException();

        return await _mediator.Send(new GetProfilesRequest(), cancellationToken);
    }

    [SwaggerOperation(
        Summary = "Get Current Profile",
        Description = @"Get Current Profile"
    )]
    [HttpGet("current", Name = "getCurrentProfile")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetCurremtProfileResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetCurremtProfileResponse>> GetCurrent(CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetCurremtProfileRequest(), cancellationToken);
    }

    [SwaggerOperation(
        Summary = "Get Profile by id",
        Description = @"Get Profile by id"
    )]
    [HttpGet("{profileId:guid}", Name = "getProfileById")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetProfileByIdResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetProfileByIdResponse>> GetById([FromRoute] Guid profileId, CancellationToken cancellationToken)
    {
        var request = new GetProfileByIdRequest() { ProfileId = profileId };

        var response = await _mediator.Send(request, cancellationToken);

        if (response.Profile == null)
        {
            return new NotFoundObjectResult(request.ProfileId);
        }

        return response;
    }

    [SwaggerOperation(
        Summary = "Delete Profile",
        Description = @"Delete Profile"
    )]
    [HttpDelete("{profileId:guid}", Name = "deleteProfile")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(DeleteProfileResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<DeleteProfileResponse>> Delete([FromRoute] Guid profileId, CancellationToken cancellationToken)
    {
        var request = new DeleteProfileRequest() { ProfileId = profileId };

        return await _mediator.Send(request, cancellationToken);
    }

}


