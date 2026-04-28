using Asp.Versioning;
using Commitments.Shared;
using Identity.Features.Profile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Net.Mime;

namespace Identity.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/{version:apiVersion}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class ProfileController
{
    private readonly ISender _sender;
    private readonly ILogger<ProfileController> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProfileController(ISender sender, ILogger<ProfileController> logger, IHttpContextAccessor httpContextAccessor)
    {
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    [SwaggerOperation(Summary = "Create Profile", Description = "Create Profile")]
    [HttpPost(Name = "createProfile")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(CreateProfileResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<CreateProfileResponse>> Create([FromBody] CreateProfileRequest request, CancellationToken cancellationToken)
    {
        return await _sender.Send(request, cancellationToken);
    }

    [SwaggerOperation(Summary = "Get Profiles", Description = "Get Profiles")]
    [HttpGet(Name = "getProfiles")]
    [Authorize]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetProfilesResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetProfilesResponse>> Get(CancellationToken cancellationToken)
    {
        return await _sender.Send(
            new GetProfilesRequest { UserId = _httpContextAccessor.GetUserId() },
            cancellationToken);
    }

    [SwaggerOperation(Summary = "Get Profile by id", Description = "Get Profile by id")]
    [HttpGet("{profileId:guid}", Name = "getProfileById")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetProfileByIdResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetProfileByIdResponse>> GetById([FromRoute] Guid profileId, CancellationToken cancellationToken)
    {
        var request = new GetProfileByIdRequest() { ProfileId = profileId };
        var response = await _sender.Send(request, cancellationToken);
        if (response.Profile == null) return new NotFoundObjectResult(request.ProfileId);
        return response;
    }

    [SwaggerOperation(Summary = "Delete Profile", Description = "Delete Profile")]
    [HttpDelete("{profileId:guid}", Name = "deleteProfile")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(DeleteProfileResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<DeleteProfileResponse>> Delete([FromRoute] Guid profileId, CancellationToken cancellationToken)
    {
        return await _sender.Send(new DeleteProfileRequest() { ProfileId = profileId }, cancellationToken);
    }

    [SwaggerOperation(Summary = "Get current profile", Description = "Returns the caller's first non-deleted profile.")]
    [HttpGet("current", Name = "getCurrentProfile")]
    [Authorize]
    [ProducesResponseType(typeof(GetCurrentProfileResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<ActionResult<GetCurrentProfileResponse>> GetCurrent(CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new GetCurrentProfileRequest { UserId = _httpContextAccessor.GetUserId() },
            cancellationToken);

        if (response.Profile is null) return new NotFoundResult();
        return response;
    }
}
