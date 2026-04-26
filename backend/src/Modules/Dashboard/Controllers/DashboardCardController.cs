using Asp.Versioning;
using Dashboard.Features.DashboardCard;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Net.Mime;

namespace Dashboard.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/{version:apiVersion}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class DashboardCardController
{
    private readonly ISender _sender;
    private readonly ILogger<DashboardCardController> _logger;

    public DashboardCardController(ISender sender, ILogger<DashboardCardController> logger)
    {
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [SwaggerOperation(Summary = "Update DashboardCard")]
    [HttpPut(Name = "updateDashboardCard")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UpdateDashboardCardResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<UpdateDashboardCardResponse>> Update([FromBody] UpdateDashboardCardRequest request, CancellationToken cancellationToken)
    {
        return await _sender.Send(request, cancellationToken);
    }

    [SwaggerOperation(Summary = "Create DashboardCard")]
    [HttpPost(Name = "createDashboardCard")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(CreateDashboardCardResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<CreateDashboardCardResponse>> Create([FromBody] CreateDashboardCardRequest request, CancellationToken cancellationToken)
    {
        return await _sender.Send(request, cancellationToken);
    }

    [SwaggerOperation(Summary = "Save DashboardCard Range")]
    [HttpPost("range", Name = "saveDashboardCardRange")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(SaveDashboardCardRangeResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<SaveDashboardCardRangeResponse>> SaveRange([FromBody] SaveDashboardCardRangeRequest request, CancellationToken cancellationToken)
    {
        return await _sender.Send(request, cancellationToken);
    }

    [SwaggerOperation(Summary = "Get DashboardCards")]
    [HttpGet(Name = "getDashboardCards")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetDashboardCardsResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetDashboardCardsResponse>> Get(CancellationToken cancellationToken)
    {
        return await _sender.Send(new GetDashboardCardsRequest(), cancellationToken);
    }

    [SwaggerOperation(Summary = "Get DashboardCard by id")]
    [HttpGet("{dashboardCardId:guid}", Name = "getDashboardCardById")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetDashboardCardByIdResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetDashboardCardByIdResponse>> GetById([FromRoute] Guid dashboardCardId, CancellationToken cancellationToken)
    {
        var request = new GetDashboardCardByIdRequest() { DashboardCardId = dashboardCardId };
        var response = await _sender.Send(request, cancellationToken);
        if (response.DashboardCard == null) return new NotFoundObjectResult(request.DashboardCardId);
        return response;
    }

    [SwaggerOperation(Summary = "Delete DashboardCard")]
    [HttpDelete("{dashboardCardId:guid}", Name = "deleteDashboardCard")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(DeleteDashboardCardResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<DeleteDashboardCardResponse>> Delete([FromRoute] Guid dashboardCardId, CancellationToken cancellationToken)
    {
        return await _sender.Send(new DeleteDashboardCardRequest() { DashboardCardId = dashboardCardId }, cancellationToken);
    }
}
