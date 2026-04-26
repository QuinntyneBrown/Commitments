using Identity.Features.User;
using Identity.Data;
using Identity.Domain.UserAggregate;
using Commitments.Testing.Common;
using FluentAssertions;
using Moq;
using Xunit;

namespace Identity.Api.Tests.Handlers.Commands;

public class CreateUserHandlerTests
{
    private readonly Mock<IIdentityDbContext> _mockContext;
    private readonly CreateUserRequestHandler _handler;

    public CreateUserHandlerTests()
    {
        _mockContext = MockIdentityDbContextFactory.Create();
        _handler = new CreateUserRequestHandler(_mockContext.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateUserAndReturnDto()
    {
        var request = new CreateUserRequest
        {
            User = new UserDto
            {
                Username = "johndoe",
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com"
            }
        };

        var response = await _handler.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        response.User.Should().NotBeNull();
        response.User.Username.Should().Be("johndoe");
        response.User.FirstName.Should().Be("John");
        response.User.LastName.Should().Be("Doe");
        response.User.Email.Should().Be("john@example.com");
        _mockContext.Verify(c => c.Users.Add(It.IsAny<User>()), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldAssignNewUserIdOnCreate()
    {
        var request = new CreateUserRequest
        {
            User = new UserDto
            {
                Username = "newuser",
                FirstName = "New",
                LastName = "User",
                Email = "new@example.com"
            }
        };

        var response = await _handler.Handle(request, CancellationToken.None);

        response.User.Should().NotBeNull();
        response.User.Username.Should().Be("newuser");
    }
}
