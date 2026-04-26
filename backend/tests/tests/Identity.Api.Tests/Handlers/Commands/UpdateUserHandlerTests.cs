using Identity.Api.Features.User;
using Identity.Core;
using Identity.Core.Model.UserAggregate;
using Commitments.Testing.Common;
using Commitments.Testing.Common.Builders;
using FluentAssertions;
using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace Identity.Api.Tests.Handlers.Commands;

public class UpdateUserHandlerTests
{
    private readonly Mock<IIdentityDbContext> _mockContext;
    private readonly UpdateUserRequestHandler _handler;

    public UpdateUserHandlerTests()
    {
        _mockContext = MockIdentityDbContextFactory.Create();
        _handler = new UpdateUserRequestHandler(_mockContext.Object);
    }

    [Fact]
    public async Task Handle_WhenUserExists_ShouldUpdateAndReturnDto()
    {
        var userId = Guid.NewGuid();
        var existingUser = new UserBuilder()
            .WithUserId(userId)
            .WithUsername("olduser")
            .WithFirstName("Old")
            .WithLastName("Name")
            .WithEmail("old@example.com")
            .Build();

        _mockContext.Setup(c => c.Users.FindAsync(userId))
            .ReturnsAsync(existingUser);

        var request = new UpdateUserRequest
        {
            User = new UserDto
            {
                UserId = userId,
                Username = "updateduser",
                FirstName = "Updated",
                LastName = "User",
                Email = "updated@example.com"
            }
        };

        var response = await _handler.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        response.User.Should().NotBeNull();
        response.User.Username.Should().Be("updateduser");
        response.User.FirstName.Should().Be("Updated");
        response.User.LastName.Should().Be("User");
        response.User.Email.Should().Be("updated@example.com");
        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenUserNotFound_ShouldReturnEmptyResponse()
    {
        var userId = Guid.NewGuid();
        _mockContext.Setup(c => c.Users.FindAsync(userId))
            .ReturnsAsync((User?)null);

        var request = new UpdateUserRequest
        {
            User = new UserDto
            {
                UserId = userId,
                Username = "nonexistent"
            }
        };

        var response = await _handler.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        response.User.Should().BeNull();
        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
