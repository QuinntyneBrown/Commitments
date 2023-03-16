using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace Commitments.Api.Features.FrequencyTypes;

[Authorize]
[ApiController]
[Route("api/frequencyTypes")]
public class FrequencyTypesController
{
    private readonly IMediator _mediator;

    public FrequencyTypesController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<SaveFrequencyTypeCommandResponse>> Save(SaveFrequencyTypeCommandRequest request)
        => await _mediator.Send(request);

    [HttpDelete("{frequencyTypeId}")]
    public async Task Remove(RemoveFrequencyTypeCommandRequest request)
        => await _mediator.Send(request);            

    [HttpGet("{frequencyTypeId}")]
    public async Task<ActionResult<GetFrequencyTypeByIdQueryResponse>> GetById([FromRoute]GetFrequencyTypeByIdQueryRequest request)
        => await _mediator.Send(request);

    [HttpGet]
    public async Task<ActionResult<GetFrequencyTypesQueryResponse>> Get()
        => await _mediator.Send(new GetFrequencyTypesQueryRequest());
}
