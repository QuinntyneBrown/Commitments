// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel;
using FluentAssertions;
using Xunit;

namespace Commitments.Core.Tests.AggregateModel;

public class ProfileTests
{
    [Fact]
    public void Profile_ShouldSetProfileId()
    {
        // Arrange
        var profile = new Profile();
        var id = Guid.NewGuid();

        // Act
        profile.ProfileId = id;

        // Assert
        profile.ProfileId.Should().Be(id);
    }

    [Fact]
    public void Profile_ShouldInheritFromBaseEntity()
    {
        // Arrange & Act
        var profile = new Profile();
        var now = DateTime.UtcNow;

        profile.CreatedOn = now;
        profile.LastModifiedOn = now;
        profile.IsDeleted = true;

        // Assert
        profile.CreatedOn.Should().Be(now);
        profile.LastModifiedOn.Should().Be(now);
        profile.IsDeleted.Should().BeTrue();
    }
}
