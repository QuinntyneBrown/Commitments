using Commitments.Core.Entities;

namespace Commitments.API.Features.Profiles
{
    public class ProfileApiModel
    {        
        public int ProfileId { get; set; }
        public string Name { get; set; }
        public string AvatarUrl { get; set; }
        public static ProfileApiModel FromProfile(Profile profile)
            => new ProfileApiModel
            {
                ProfileId = profile.ProfileId,
                Name = profile.Name,
                AvatarUrl = profile.AvatarUrl
            };
    }
}
