using Commitments.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace Commitments.Api.Features.Commitments;

[Authorize]
[ApiController]
[Route("api/commitments")]
public class CommitmentsController
{
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CommitmentsController(IHttpContextAccessor httpContextAccessor, IMediator mediator)
    {
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    public async Task<ActionResult<SaveCommitmentCommandResponse>> Save(SaveCommitmentCommandRequest request) {
        request.Commitment.ProfileId = _httpContextAccessor.GetProfileIdFromClaims();
        return await _mediator.Send(request);
    }

    [HttpDelete("{commitmentId}")]
    public async Task Remove(RemoveCommitmentCommandRequest request)
        => await _mediator.Send(request);            

    [HttpGet("{CommitmentId}")]
    public async Task<ActionResult<GetCommitmentByIdQueryResponse>> GetById([FromRoute]GetCommitmentByIdQueryRequest request)
        => await _mediator.Send(request);

    [HttpGet]
    public async Task<ActionResult<GetCommitmentsQueryResponse>> Get()
        => await _mediator.Send(new GetCommitmentsQueryRequest());

    [HttpGet("personal")]
    public async Task<ActionResult<GetPersonalCommitmentsQueryResponse>> GetPersonal()
        => await _mediator.Send(new GetPersonalCommitmentsQueryRequest() { ProfileId = _httpContextAccessor.GetProfileIdFromClaims() });

    [HttpGet("daily")]
    public async Task<ActionResult<GetDailyCommitmentsQueryResponse>> GetDaily() 
        => await _mediator.Send(new GetDailyCommitmentsQueryRequest() { ProfileId = _httpContextAccessor.GetProfileIdFromClaims() });
}
