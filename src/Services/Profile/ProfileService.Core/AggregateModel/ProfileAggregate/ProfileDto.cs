// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace ProfileService.Core.AggregateModel.ProfileAggregate;

public class ProfileDto
{
    public Guid ProfileId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}


