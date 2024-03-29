// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Net.Http.Headers;

namespace DigitalAssetService.Core.Helpers;

public static class MultipartRequestHelper
{
    public static string GetBoundary(Microsoft.Net.Http.Headers.MediaTypeHeaderValue contentType, int lengthLimit)
    {
        var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary);
        if (string.IsNullOrWhiteSpace(boundary.ToString()))
        {
            throw new InvalidDataException("Missing content-type boundary.");
        }

        if (boundary.Length > lengthLimit)
        {
            throw new InvalidDataException(
                $"Multipart boundary length limit {lengthLimit} exceeded.");
        }

        return boundary.ToString();
    }

    public static bool IsMultipartContentType(string contentType)
    {
        return !string.IsNullOrEmpty(contentType)
               && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
    }

    public static bool HasFormDataContentDisposition(Microsoft.Net.Http.Headers.ContentDispositionHeaderValue contentDisposition)
    {
        return contentDisposition != null
               && contentDisposition.DispositionType.Equals("form-data")
               && string.IsNullOrEmpty(contentDisposition.FileName.ToString())
               && string.IsNullOrEmpty(contentDisposition.FileNameStar.ToString());
    }

    public static bool HasFileContentDisposition(Microsoft.Net.Http.Headers.ContentDispositionHeaderValue contentDisposition)
    {
        return contentDisposition != null
               && contentDisposition.DispositionType.Equals("form-data")
               && (!string.IsNullOrEmpty(contentDisposition.FileName.ToString())
                   || !string.IsNullOrEmpty(contentDisposition.FileNameStar.ToString()));
    }
}