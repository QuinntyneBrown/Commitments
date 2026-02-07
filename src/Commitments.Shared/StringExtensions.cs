// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Commitments.Shared;

public static class StringExtensions
{
    public static string ToCamelCase(this string value)
    {
        if (string.IsNullOrEmpty(value) || char.IsLower(value[0]))
            return value;

        return char.ToLowerInvariant(value[0]) + value.Substring(1);
    }

    public static string GenerateSlug(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        var slug = value.ToLowerInvariant();
        slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[^a-z0-9\s-]", "");
        slug = System.Text.RegularExpressions.Regex.Replace(slug, @"\s+", " ").Trim();
        slug = slug.Replace(" ", "-");

        return slug;
    }
}
