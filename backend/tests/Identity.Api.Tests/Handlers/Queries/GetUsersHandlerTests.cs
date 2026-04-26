using Identity.Features.User;
using Identity.Data;
using Identity.Domain.UserAggregate;
using Commitments.Testing.Common;
using FluentAssertions;
using Moq;
using Xunit;

namespace Identity.Api.Tests.Handlers.Queries;

public class GetUsersHandlerTests
{
    [Fact]
    public async Task Handle_WhenNoUsers_ShouldReturnEmptyList()
    {
        var mockContext = MockIdentityDbContextFactory.Create();
        var emptyList = new List<User>();
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(emptyList);
        mockContext.Setup(c => c.Users).Returns(mockDbSet.Object);

        var handler = new GetUsersRequestHandler(mockContext.Object);

        var response = await handler.Handle(new GetUsersRequest(), CancellationToken.None);

        response.Should().NotBeNull();
        response.Users.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_WhenUsersExist_ShouldReturnPopulatedList()
    {
        var mockContext = MockIdentityDbContextFactory.Create();
        var users = new List<User>
        {
            new User { UserId = Guid.NewGuid(), Username = "user1", FirstName = "First1", LastName = "Last1", Email = "user1@example.com" },
            new User { UserId = Guid.NewGuid(), Username = "user2", FirstName = "First2", LastName = "Last2", Email = "user2@example.com" }
        };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(users);
        mockContext.Setup(c => c.Users).Returns(mockDbSet.Object);

        var handler = new GetUsersRequestHandler(mockContext.Object);

        var response = await handler.Handle(new GetUsersRequest(), CancellationToken.None);

        response.Should().NotBeNull();
        response.Users.Should().HaveCount(2);
        response.Users[0].Username.Should().Be("user1");
        response.Users[1].Username.Should().Be("user2");
    }
}
