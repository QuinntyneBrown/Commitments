using Commitments.Core.Entities;

namespace Commitments.API.Features.Profiles
{
    public class ProfileApiModel
    {        
        public int ProfileId { get; set; }
        public string Name { get; set; }

        public static ProfileApiModel FromProfile(Profile profile)
        {
            var model = new ProfileApiModel();
            model.ProfileId = profile.ProfileId;
            model.Name = profile.Name;
            return model;
        }
    }
}
