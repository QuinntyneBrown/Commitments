// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Commitments.Core.Services.Kernel;

public static class StringExtensions
{
    public static string ToCamelCase(this string value)
    {
        if (string.IsNullOrEmpty(value) || char.IsLower(value[0]))
            return value;

        return char.ToLowerInvariant(value[0]) + value.Substring(1);
    }
}
