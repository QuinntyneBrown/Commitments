// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using DigitalAssetService.Core.AggregateModel.DigitalAssetAggregate.Commands;
using DigitalAssetService.Core.AggregateModel.DigitalAssetAggregate.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Net.Mime;

namespace DigitalAssetService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class DigitalAssetController
{
    private readonly ISender _sender;

    private readonly ILogger<DigitalAssetController> _logger;

    public DigitalAssetController(ISender sender, ILogger<DigitalAssetController> logger)
    {
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [SwaggerOperation(
        Summary = "Upload digital asset",
        Description = @"Upload digital asset"
    )]
    [HttpPost("upload"), DisableRequestSizeLimit]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UploadDigitalAssetResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<UploadDigitalAssetResponse>> Post(string command)
    {

        return await _sender.Send(new UploadDigitalAssetRequest());
    }

    [SwaggerOperation(
        Summary = "Update DigitalAsset",
        Description = @"Update DigitalAsset"
    )]
    [HttpPut(Name = "updateDigitalAsset")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UpdateDigitalAssetResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<UpdateDigitalAssetResponse>> Update([FromBody] UpdateDigitalAssetRequest request, CancellationToken cancellationToken)
    {
        return await _sender.Send(request, cancellationToken);
    }

    [SwaggerOperation(
        Summary = "Create DigitalAsset",
        Description = @"Create DigitalAsset"
    )]
    [HttpPost(Name = "createDigitalAsset")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(CreateDigitalAssetResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<CreateDigitalAssetResponse>> Create([FromBody] CreateDigitalAssetRequest request, CancellationToken cancellationToken)
    {
        return await _sender.Send(request, cancellationToken);
    }

    [SwaggerOperation(
        Summary = "Get DigitalAssets",
        Description = @"Get DigitalAssets"
    )]
    [HttpGet(Name = "getDigitalAssets")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetDigitalAssetsResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetDigitalAssetsResponse>> Get(CancellationToken cancellationToken)
    {
        return await _sender.Send(new GetDigitalAssetsRequest(), cancellationToken);
    }

    [SwaggerOperation(
        Summary = "Get DigitalAssetId  by id",
        Description = @"Get DigitalAssetId by id"
    )]
    [HttpGet("{digitalAssetId:guid}", Name = "getDigitalAssetIdById")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetDigitalAssetByIdResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetDigitalAssetByIdResponse>> GetById([FromRoute] Guid digitalAssetId, CancellationToken cancellationToken)
    {
        var request = new GetDigitalAssetByIdRequest() { DigitalAssetId = digitalAssetId };

        var response = await _sender.Send(request, cancellationToken);

        if (response.DigitalAsset == null)
        {
            return new NotFoundObjectResult(request.DigitalAssetId);
        }

        return response;
    }

    [SwaggerOperation(
        Summary = "Delete DigitalAsset",
        Description = @"Delete DigitalAsset"
    )]
    [HttpDelete("{digitalAssetId:guid}", Name = "deleteDigitalAsset")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(DeleteDigitalAssetResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<DeleteDigitalAssetResponse>> Delete([FromRoute] Guid digitalAssetId, CancellationToken cancellationToken)
    {
        var request = new DeleteDigitalAssetRequest() { DigitalAssetId = digitalAssetId };

        return await _sender.Send(request, cancellationToken);
    }

}