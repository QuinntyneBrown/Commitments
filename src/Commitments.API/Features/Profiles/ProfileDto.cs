// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel;


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

