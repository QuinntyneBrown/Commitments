using Commitments.Features.Tag;
using Commitments.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Commitments.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/tags")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class TagController
{
    private readonly ISender _sender;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TagController(ISender sender, IHttpContextAccessor httpContextAccessor)
    {
        _sender = sender;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetTagsResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetTagsResponse>> Get()
        => await _sender.Send(new GetTagsRequest { ProfileId = _httpContextAccessor.GetProfileId() });

    [HttpGet("slug/{slug}")]
    [ProducesResponseType(typeof(GetTagBySlugResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<GetTagBySlugResponse>> GetBySlug([FromRoute] string slug)
    {
        var response = await _sender.Send(new GetTagBySlugRequest { ProfileId = _httpContextAccessor.GetProfileId(), Slug = slug });
        if (response.Tag is null) return new NotFoundResult();
        return response;
    }

    [HttpPost]
    [ProducesResponseType(typeof(SaveTagResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<SaveTagResponse>> Save([FromBody] SaveTagRequest request)
    {
        request.Tag.ProfileId = _httpContextAccessor.GetProfileId();
        return await _sender.Send(request);
    }

    [HttpDelete("{tagId:guid}")]
    public async Task Remove([FromRoute] Guid tagId)
        => await _sender.Send(new RemoveTagRequest { TagId = tagId });
}
