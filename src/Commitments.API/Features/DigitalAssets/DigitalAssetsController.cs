using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Commitments.API.Features.DigitalAssets
{
    [Authorize]
    [ApiController]
    [Route("api/digitalAssets")]
    public class DigitalAssetsController
    {
        private readonly IMediator _mediator;

        public DigitalAssetsController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<SaveDigitalAssetCommand.Response>> Save(SaveDigitalAssetCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpDelete("{DigitalAsset.DigitalAssetId}")]
        public async Task Remove(RemoveDigitalAssetCommand.Request request)
            => await _mediator.Send(request);            

        [HttpGet("{DigitalAssetId}")]
        public async Task<ActionResult<GetDigitalAssetByIdQuery.Response>> GetById([FromRoute]GetDigitalAssetByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetDigitalAssetsQuery.Response>> Get()
            => await _mediator.Send(new GetDigitalAssetsQuery.Request());
    }
}
