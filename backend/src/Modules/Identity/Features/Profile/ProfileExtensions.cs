using Identity.Domain.ProfileAggregate;
using Microsoft.EntityFrameworkCore;

namespace Identity.Features.Profile;

public static class ProfileExtensions
{
    public static ProfileDto ToDto(this Identity.Domain.ProfileAggregate.Profile profile)
    {
        return new ProfileDto { ProfileId = profile.ProfileId };
    }

    public static async Task<List<ProfileDto>> ToDtosAsync(this IQueryable<Identity.Domain.ProfileAggregate.Profile> profiles, CancellationToken cancellationToken)
    {
        return await profiles.Select(x => x.ToDto()).ToListAsync(cancellationToken);
    }
}
