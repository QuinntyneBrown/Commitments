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
    public async Task<ActionResult<SaveDigitalAssetCommandResponse>> Save(SaveDigitalAssetCommandRequest request)
        => await _mediator.Send(request);

    [HttpGet("range")]
    public async Task<ActionResult<GetDigitalAssetsByIdsQueryResponse>> GetByIds([FromQuery]GetDigitalAssetsByIdsQueryRequest request)
        => await _mediator.Send(request);

    [HttpDelete("{digitalAssetId}")]
    public async Task Remove(RemoveDigitalAssetCommandRequest request)
        => await _mediator.Send(request);            

    [HttpGet("{digitalAssetId}")]
    public async Task<ActionResult<GetDigitalAssetByIdQueryResponse>> GetById([FromRoute]GetDigitalAssetByIdQueryRequest request)
        => await _mediator.Send(request);

    [HttpPost("upload"), DisableRequestSizeLimit]
    public async Task<ActionResult<UploadDigitalAssetCommandResponse>> Save()
        => await _mediator.Send(new UploadDigitalAssetCommandRequest());

    [AllowAnonymous]
    [HttpGet("serve/{digitalAssetId}")]
    public async Task<IActionResult> Serve([FromRoute]GetDigitalAssetByIdQueryRequest request)
    {
        var response = await _mediator.Send(request);
        return new FileContentResult(response.DigitalAsset.Bytes, response.DigitalAsset.ContentType);            
    }

    [HttpGet]
    public async Task<ActionResult<GetDigitalAssetsQueryResponse>> Get()
        => await _mediator.Send(new GetDigitalAssetsQueryRequest());
}
