using Commitments.Core.Entities;


namespace Commitments.Api.Features.Profiles;

public class ProfileDto
{        
    public int ProfileId { get; set; }
    public string Name { get; set; }
    public string AvatarUrl { get; set; }
    public static ProfileDto FromProfile(Profile profile)
        => new ProfileDto
        {
            ProfileId = profile.ProfileId,
            Name = profile.Name,
            AvatarUrl = profile.AvatarUrl
        };
}
