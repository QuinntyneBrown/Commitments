// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;

namespace Commitments.Core.Extensions;

public static class StringExtensions
{
    public static string GenerateSlug(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        // Convert to lower case
        var slug = value.ToLowerInvariant();

        // Remove invalid characters
        slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[^a-z0-9\s-]", "");

        // Replace multiple spaces with a single space
        slug = System.Text.RegularExpressions.Regex.Replace(slug, @"\s+", " ").Trim();

        // Replace spaces with hyphens
        slug = slug.Replace(" ", "-");

        return slug;
    }
}