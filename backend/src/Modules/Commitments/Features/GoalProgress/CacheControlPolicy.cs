// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Commitments.Features.GoalProgress;

public static class CacheControlPolicy
{
    public const string NoStore = "no-store";
    public const string PublicHistorical = "public, max-age=300";

    private static readonly TimeSpan FreshnessWindow = TimeSpan.FromMinutes(1);

    public static string For(DateTimeOffset? asOf, DateTimeOffset utcNow)
    {
        if (asOf is null) return NoStore;
        var age = utcNow - asOf.Value;
        return age > FreshnessWindow ? PublicHistorical : NoStore;
    }
}
