using MediatR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Commitments.Api.Features.DigitalAssets;

[Authorize]
[ApiController]
[Route("api/digitalAssets")]
public class DigitalAssetsController
{
    private readonly IMediator _mediator;

    public DigitalAssetsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<SaveDigitalAssetResponse>> Save(SaveDigitalAssetRequest request)
        => await _mediator.Send(request);

    [HttpGet("range")]
    public async Task<ActionResult<GetDigitalAssetsByIdsResponse>> GetByIds([FromQuery]GetDigitalAssetsByIdsRequest request)
        => await _mediator.Send(request);

    [HttpDelete("{digitalAssetId}")]
    public async Task Remove(RemoveDigitalAssetRequest request)
        => await _mediator.Send(request);            

    [HttpGet("{digitalAssetId}")]
    public async Task<ActionResult<GetDigitalAssetByIdResponse>> GetById([FromRoute]GetDigitalAssetByIdRequest request)
        => await _mediator.Send(request);

    [HttpPost("upload"), DisableRequestSizeLimit]
    public async Task<ActionResult<UploadDigitalAssetResponse>> Save()
        => await _mediator.Send(new UploadDigitalAssetRequest());

    [AllowAnonymous]
    [HttpGet("serve/{digitalAssetId}")]
    public async Task<IActionResult> Serve([FromRoute]GetDigitalAssetByIdRequest request)
    {
        var response = await _mediator.Send(request);
        return new FileContentResult(response.DigitalAsset.Bytes, response.DigitalAsset.ContentType);            
    }

    [HttpGet]
    public async Task<ActionResult<GetDigitalAssetsResponse>> Get()
        => await _mediator.Send(new GetDigitalAssetsRequest());
}
