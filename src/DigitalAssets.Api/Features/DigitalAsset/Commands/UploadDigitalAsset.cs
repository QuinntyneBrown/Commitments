using Commitments.Shared;
using DigitalAssets.Core;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DigitalAssets.Api.Features.DigitalAsset;

public class UploadDigitalAssetRequest : IRequest<UploadDigitalAssetResponse> { }

public class UploadDigitalAssetResponse : ResponseBase
{
    public List<DigitalAssetDto> DigitalAssets { get; set; } = new();
}

public class UploadDigitalAssetRequestHandler : IRequestHandler<UploadDigitalAssetRequest, UploadDigitalAssetResponse>
{
    private readonly IDigitalAssetsDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UploadDigitalAssetRequestHandler(IDigitalAssetsDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<UploadDigitalAssetResponse> Handle(UploadDigitalAssetRequest request, CancellationToken cancellationToken)
    {
        var httpContext = _httpContextAccessor.HttpContext!;
        var digitalAssets = new List<DigitalAssets.Core.Model.DigitalAssetAggregate.DigitalAsset>();

        foreach (var file in httpContext.Request.Form.Files)
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream, cancellationToken);

            var digitalAsset = new DigitalAssets.Core.Model.DigitalAssetAggregate.DigitalAsset
            {
                Name = file.FileName,
                Bytes = stream.ToArray(),
                ContentType = file.ContentType
            };

            _context.DigitalAssets.Add(digitalAsset);
            digitalAssets.Add(digitalAsset);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new UploadDigitalAssetResponse
        {
            DigitalAssets = digitalAssets.Select(x => x.ToDto()).ToList()
        };
    }
}
