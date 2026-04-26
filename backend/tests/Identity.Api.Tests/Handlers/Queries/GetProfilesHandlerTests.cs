using Identity.Features.Profile;
using Identity.Data;
using Identity.Domain.ProfileAggregate;
using Commitments.Testing.Common;
using FluentAssertions;
using Moq;
using Xunit;

namespace Identity.Api.Tests.Handlers.Queries;

public class GetProfilesHandlerTests
{
    [Fact]
    public async Task Handle_WhenNoProfiles_ShouldReturnEmptyList()
    {
        var mockContext = MockIdentityDbContextFactory.Create();
        var emptyList = new List<Profile>();
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(emptyList);
        mockContext.Setup(c => c.Profiles).Returns(mockDbSet.Object);

        var handler = new GetProfilesRequestHandler(mockContext.Object);

        var response = await handler.Handle(new GetProfilesRequest(), CancellationToken.None);

        response.Should().NotBeNull();
        response.Profiles.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_WhenProfilesExist_ShouldReturnPopulatedList()
    {
        var mockContext = MockIdentityDbContextFactory.Create();
        var profiles = new List<Profile>
        {
            new Profile { ProfileId = Guid.NewGuid() },
            new Profile { ProfileId = Guid.NewGuid() }
        };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(profiles);
        mockContext.Setup(c => c.Profiles).Returns(mockDbSet.Object);

        var handler = new GetProfilesRequestHandler(mockContext.Object);

        var response = await handler.Handle(new GetProfilesRequest(), CancellationToken.None);

        response.Should().NotBeNull();
        response.Profiles.Should().HaveCount(2);
    }
}
