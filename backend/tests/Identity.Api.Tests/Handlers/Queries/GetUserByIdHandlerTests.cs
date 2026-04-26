using Identity.Api.Features.User;
using Identity.Core;
using Identity.Core.Model.UserAggregate;
using Commitments.Testing.Common;
using Commitments.Testing.Common.Builders;
using FluentAssertions;
using Moq;
using Xunit;

namespace Identity.Api.Tests.Handlers.Queries;

public class GetUserByIdHandlerTests
{
    private readonly Mock<IIdentityDbContext> _mockContext;
    private readonly GetUserByIdRequestHandler _handler;

    public GetUserByIdHandlerTests()
    {
        _mockContext = MockIdentityDbContextFactory.Create();
        _handler = new GetUserByIdRequestHandler(_mockContext.Object);
    }

    [Fact]
    public async Task Handle_WhenUserExists_ShouldReturnUserDto()
    {
        var userId = Guid.NewGuid();
        var existingUser = new UserBuilder()
            .WithUserId(userId)
            .WithUsername("testuser")
            .WithFirstName("Test")
            .WithLastName("User")
            .WithEmail("test@example.com")
            .Build();

        _mockContext.Setup(c => c.Users.FindAsync(userId))
            .ReturnsAsync(existingUser);

        var request = new GetUserByIdRequest { UserId = userId };

        var response = await _handler.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        response.User.Should().NotBeNull();
        response.User!.UserId.Should().Be(userId);
        response.User.Username.Should().Be("testuser");
        response.User.FirstName.Should().Be("Test");
        response.User.LastName.Should().Be("User");
        response.User.Email.Should().Be("test@example.com");
    }

    [Fact]
    public async Task Handle_WhenUserNotFound_ShouldReturnNullUser()
    {
        var userId = Guid.NewGuid();
        _mockContext.Setup(c => c.Users.FindAsync(userId))
            .ReturnsAsync((User?)null);

        var request = new GetUserByIdRequest { UserId = userId };

        var response = await _handler.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        response.User.Should().BeNull();
    }
}
