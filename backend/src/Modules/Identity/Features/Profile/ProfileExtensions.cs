using Identity.Core.Model.ProfileAggregate;
using Microsoft.EntityFrameworkCore;

namespace Identity.Api.Features.Profile;

public static class ProfileExtensions
{
    public static ProfileDto ToDto(this Identity.Core.Model.ProfileAggregate.Profile profile)
    {
        return new ProfileDto { ProfileId = profile.ProfileId };
    }

    public static async Task<List<ProfileDto>> ToDtosAsync(this IQueryable<Identity.Core.Model.ProfileAggregate.Profile> profiles, CancellationToken cancellationToken)
    {
        return await profiles.Select(x => x.ToDto()).ToListAsync(cancellationToken);
    }
}
