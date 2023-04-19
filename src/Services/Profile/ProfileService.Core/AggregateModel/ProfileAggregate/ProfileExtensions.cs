// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace ProfileService.Core.AggregateModel.ProfileAggregate;

public static class ProfileExtensions
{
    public static ProfileDto ToDto(this Profile profile)
    {
        return new ProfileDto
        {
            ProfileId = profile.ProfileId,
            Name = profile.Name,
            Email = profile.Email,
        };

    }

    public async static Task<List<ProfileDto>> ToDtosAsync(this IQueryable<Profile> profiles, CancellationToken cancellationToken)
    {
        return await profiles.Select(x => x.ToDto()).ToListAsync(cancellationToken);
    }

}