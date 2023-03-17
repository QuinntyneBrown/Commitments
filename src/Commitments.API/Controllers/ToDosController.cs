using Commitments.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace Commitments.Api.Features.ToDos;

[Authorize]
[ApiController]
[Route("api/toDos")]
public class ToDosController
{
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ToDosController(IHttpContextAccessor httpContextAccessor, IMediator mediator) {
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    public async Task<ActionResult<SaveToDoResponse>> Save(SaveToDoRequest request) {
        request.ToDo.ProfileId = _httpContextAccessor.GetProfileIdFromClaims();
        return await _mediator.Send(request);
    }

    [HttpDelete("{toDoId}")]
    public async Task Remove([FromRoute]RemoveToDoRequest request)
        => await _mediator.Send(request);            

    [HttpGet("{toDoId}")]
    public async Task<ActionResult<GetToDoByIdResponse>> GetById([FromRoute]GetToDoByIdRequest request)
        => await _mediator.Send(request);

    [HttpGet("outstanding")]
    public async Task<ActionResult<GetOutstandingToDosResponse>> GetOutstanding()
        => await _mediator.Send(new GetOutstandingToDosRequest() { ProfileId = _httpContextAccessor.GetProfileIdFromClaims() });

    [HttpGet]
    public async Task<ActionResult<GetToDosResponse>> Get()
        => await _mediator.Send(new GetToDosRequest() { ProfileId = _httpContextAccessor.GetProfileIdFromClaims() });

}
