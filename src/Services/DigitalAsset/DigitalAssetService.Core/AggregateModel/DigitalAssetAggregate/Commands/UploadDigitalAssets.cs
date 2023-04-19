// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using DigitalAssetService.Core.Helpers;
using FluentValidation;
using Kernel;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;


namespace DigitalAssetService.Core.AggregateModel.DigitalAssetAggregate.Commands;

public class UploadDigitalAssetsRequestValidator : AbstractValidator<UploadDigitalAssetsRequest> { }

public class UploadDigitalAssetsRequest : IRequest<UploadDigitalAssetsResponse> { }


public class UploadDigitalAssetsResponse : ResponseBase
{
    public required List<Guid> DigitalAssetIds { get; set; }
}


public class UploadDigitalAssetRequestHandler : IRequestHandler<UploadDigitalAssetsRequest, UploadDigitalAssetsResponse>
{
    private readonly ILogger<UploadDigitalAssetRequestHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDigitalAssetServiceDbContext _context;

    public UploadDigitalAssetRequestHandler(
        ILogger<UploadDigitalAssetRequestHandler> logger,
        IHttpContextAccessor httpContextAccessor,
        IDigitalAssetServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public async Task<UploadDigitalAssetsResponse> Handle(UploadDigitalAssetsRequest request, CancellationToken cancellationToken)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var defaultFormOptions = new FormOptions();
        var digitalAssets = new List<DigitalAsset>();

        if (!MultipartRequestHelper.IsMultipartContentType(httpContext.Request.ContentType))
            throw new Exception($"Expected a multipart request, but got {httpContext.Request.ContentType}");

        var mediaTypeHeaderValue = MediaTypeHeaderValue.Parse(httpContext.Request.ContentType);

        var boundary = MultipartRequestHelper.GetBoundary(
            mediaTypeHeaderValue,
            defaultFormOptions.MultipartBoundaryLengthLimit);

        var reader = new MultipartReader(boundary, httpContext.Request.Body);

        var section = await reader.ReadNextSectionAsync();

        while (section != null)
        {

            var digitalAsset = new DigitalAsset();

            var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out ContentDispositionHeaderValue contentDisposition);

            if (hasContentDispositionHeader)
            {
                if (MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                {
                    using (var targetStream = new MemoryStream())
                    {
                        await section.Body.CopyToAsync(targetStream);
                        var name = $"{contentDisposition.FileName}".Trim(new char[] { '"' }).Replace("&", "and");

                        digitalAsset = _context.DigitalAssets.SingleOrDefault(x => x.Name == name);

                        if (digitalAsset == null)
                        {
                            digitalAsset = new DigitalAsset();
                            digitalAsset.Name = name;
                            _context.DigitalAssets.Add(digitalAsset);
                        }

                        digitalAsset.Bytes = StreamHelper.ReadToEnd(targetStream);
                        digitalAsset.ContentType = section.ContentType;
                    }

                }
            }

            digitalAssets.Add(digitalAsset);

            section = await reader.ReadNextSectionAsync();
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new()
        {
            DigitalAssetIds = digitalAssets.Select(x => x.DigitalAssetId).ToList()
        };
    }

}