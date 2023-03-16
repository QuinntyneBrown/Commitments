using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace Commitments.Api.Features.Frequencies;

[Authorize]
[ApiController]
[Route("api/frequencies")]
public class FrequenciesController
{
    private readonly IMediator _mediator;

    public FrequenciesController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<SaveFrequencyCommandResponse>> Save(SaveFrequencyCommandRequest request)
        => await _mediator.Send(request);

    [HttpDelete("{frequencyId}")]
    public async Task Remove([FromRoute]RemoveFrequencyCommandRequest request)
        => await _mediator.Send(request);            

    [HttpGet("{frequencyId}")]
    public async Task<ActionResult<GetFrequencyByIdQueryResponse>> GetById([FromRoute]GetFrequencyByIdQueryRequest request)
        => await _mediator.Send(request);

    [HttpGet]
    public async Task<ActionResult<GetFrequenciesQueryResponse>> Get()
        => await _mediator.Send(new GetFrequenciesQueryRequest());
}
