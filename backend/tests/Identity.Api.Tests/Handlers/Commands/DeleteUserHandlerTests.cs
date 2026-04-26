using Identity.Api.Features.User;
using Identity.Core;
using Identity.Core.Model.UserAggregate;
using Commitments.Testing.Common;
using Commitments.Testing.Common.Builders;
using FluentAssertions;
using Moq;
using Xunit;

namespace Identity.Api.Tests.Handlers.Commands;

public class DeleteUserHandlerTests
{
    private readonly Mock<IIdentityDbContext> _mockContext;
    private readonly DeleteUserRequestHandler _handler;

    public DeleteUserHandlerTests()
    {
        _mockContext = MockIdentityDbContextFactory.Create();
        _handler = new DeleteUserRequestHandler(_mockContext.Object);
    }

    [Fact]
    public async Task Handle_WhenUserExists_ShouldRemoveAndSave()
    {
        var userId = Guid.NewGuid();
        var existingUser = new UserBuilder().WithUserId(userId).Build();

        _mockContext.Setup(c => c.Users.FindAsync(userId))
            .ReturnsAsync(existingUser);

        var request = new DeleteUserRequest { UserId = userId };

        var response = await _handler.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        _mockContext.Verify(c => c.Users.Remove(existingUser), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenUserNotFound_ShouldReturnResponseWithoutRemoving()
    {
        var userId = Guid.NewGuid();
        _mockContext.Setup(c => c.Users.FindAsync(userId))
            .ReturnsAsync((User?)null);

        var request = new DeleteUserRequest { UserId = userId };

        var response = await _handler.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        _mockContext.Verify(c => c.Users.Remove(It.IsAny<User>()), Times.Never);
        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
