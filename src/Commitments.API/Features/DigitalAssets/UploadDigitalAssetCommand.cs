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
using Commitments.Core.Entities;
using System.Linq;


namespace Commitments.Api.Features.DigitalAssets;

 public class UploadDigitalAssetCommandRequest : IRequest<UploadDigitalAssetCommandResponse> { }

 public class UploadDigitalAssetCommandResponse
 {
     public List<Guid> DigitalAssetIds { get;set; }
 }

 public class UploadDigitalAssetCommandHandler : IRequestHandler<UploadDigitalAssetCommandRequest, UploadDigitalAssetCommandResponse>
 {
     public IAppDbContext _context { get; set; }
     public IHttpContextAccessor _httpContextAccessor { get; set; }
     public UploadDigitalAssetCommandHandler(IAppDbContext context, IHttpContextAccessor httpContextAccessor) {
         _context = context;
         _httpContextAccessor = httpContextAccessor;
     }

     public async Task<UploadDigitalAssetCommandResponse> Handle(UploadDigitalAssetCommandRequest request, CancellationToken cancellationToken) {

         var httpContext = _httpContextAccessor.HttpContext;
         var defaultFormOptions = new FormOptions();
         var digitalAssets = new List<DigitalAsset>();

         if (!MultipartRequestHelper.IsMultipartContentType(httpContextRequest.ContentType))
             throw new Exception($"Expected a multipart request, but got {httpContextRequest.ContentType}");

         var mediaTypeHeaderValue = MediaTypeHeaderValue.Parse(httpContextRequest.ContentType);

         var boundary = MultipartRequestHelper.GetBoundary(
             mediaTypeHeaderValue,
             defaultFormOptions.MultipartBoundaryLengthLimit);

         var reader = new MultipartReader(boundary, httpContextRequest.Body);

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

         return new UploadDigitalAssetCommandResponse()
         {
             DigitalAssetIds = digitalAssets.Select(x => x.DigitalAssetId).ToList()
         };
     }
 }
