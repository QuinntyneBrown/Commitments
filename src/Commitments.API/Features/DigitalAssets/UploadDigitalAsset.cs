using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

using Commitments.Core.Interfaces;
using Commitments.Core.Helpers;
using Commitments.Core.AggregateModel;
using System.Linq;


namespace Commitments.Api.Features.DigitalAssets;

 public class UploadDigitalAssetRequest : IRequest<UploadDigitalAssetResponse> { }

 public class UploadDigitalAssetResponse
 {
     public List<Guid> DigitalAssetIds { get;set; }
 }

 public class UploadDigitalAssetCommandHandler : IRequestHandler<UploadDigitalAssetRequest, UploadDigitalAssetResponse>
 {
     public ICommimentsDbContext _context { get; set; }
     public IHttpContextAccessor _httpContextAccessor { get; set; }
     public UploadDigitalAssetCommandHandler(ICommimentsDbContext context, IHttpContextAccessor httpContextAccessor) {
         _context = context;
         _httpContextAccessor = httpContextAccessor;
     }

     public async Task<UploadDigitalAssetResponse> Handle(UploadDigitalAssetRequest request, CancellationToken cancellationToken) {

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
                         digitalAsset.Name = $"{contentDisposition.FileName}".Trim(new char[] { '"' }).Replace("&", "and");
                         digitalAsset.Bytes = StreamHelper.ReadToEnd(targetStream);
                         digitalAsset.ContentType = section.ContentType;
                     }
                 }
             }

             _context.DigitalAssets.Add(digitalAsset);

             digitalAssets.Add(digitalAsset);

             section = await reader.ReadNextSectionAsync();
         }

         await _context.SaveChangesAsync(cancellationToken);

         return new UploadDigitalAssetResponse()
         {
             DigitalAssetIds = digitalAssets.Select(x => x.DigitalAssetId).ToList()
         };
     }
 }
