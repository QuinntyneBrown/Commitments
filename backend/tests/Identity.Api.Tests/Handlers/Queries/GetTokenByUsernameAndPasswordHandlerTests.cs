using Commitments.Testing.Common;
using FluentAssertions;
using Identity.Domain.ProfileAggregate;
using Identity.Domain.UserAggregate;
using Identity.Features.User;
using Identity.Security;
using Moq;
using Xunit;

namespace Identity.Api.Tests.Handlers.Queries;

public class GetTokenByUsernameAndPasswordHandlerTests
{
    private readonly Mock<IPasswordHasher> _hasher = new();
    private readonly Mock<IAccessTokenIssuer> _tokenIssuer = new();

    [Fact]
    public async Task Handle_ShouldReturnAccessToken_WhenCredentialsMatch()
    {
        var userId = Guid.NewGuid();
        var user = new User
        {
            UserId = userId,
            Username = "quinntynebrown@gmail.com",
            PasswordHash = new byte[] { 1, 2, 3 },
            PasswordSalt = new byte[] { 4, 5, 6 }
        };

        var mockContext = MockIdentityDbContextFactory.Create(new List<User> { user });

        _hasher.Setup(h => h.Verify("password", user.PasswordSalt, user.PasswordHash)).Returns(true);
        _tokenIssuer.Setup(t => t.Issue(userId, "quinntynebrown@gmail.com")).Returns("signed-jwt");

        var handler = new GetTokenByUsernameAndPasswordRequestHandler(
            mockContext.Object, _hasher.Object, _tokenIssuer.Object);

        var response = await handler.Handle(
            new GetTokenByUsernameAndPasswordRequest
            {
                Username = "quinntynebrown@gmail.com",
                Password = "password"
            },
            CancellationToken.None);

        response.AccessToken.Should().Be("signed-jwt");
        response.ProfileId.Should().BeNull("no profile was seeded for this user");
    }

    [Fact]
    public async Task Handle_ShouldIncludeProfileId_WhenUserHasProfile()
    {
        var userId = Guid.NewGuid();
        var profileId = Guid.NewGuid();
        var user = new User
        {
            UserId = userId,
            Username = "quinntynebrown@gmail.com",
            PasswordHash = new byte[] { 1, 2, 3 },
            PasswordSalt = new byte[] { 4, 5, 6 }
        };
        var profile = new Profile { ProfileId = profileId, UserId = userId };

        var mockContext = MockIdentityDbContextFactory.Create(new List<User> { user }, new List<Profile> { profile });

        _hasher.Setup(h => h.Verify("password", user.PasswordSalt, user.PasswordHash)).Returns(true);
        _tokenIssuer.Setup(t => t.Issue(userId, "quinntynebrown@gmail.com")).Returns("signed-jwt");

        var handler = new GetTokenByUsernameAndPasswordRequestHandler(
            mockContext.Object, _hasher.Object, _tokenIssuer.Object);

        var response = await handler.Handle(
            new GetTokenByUsernameAndPasswordRequest
            {
                Username = "quinntynebrown@gmail.com",
                Password = "password"
            },
            CancellationToken.None);

        response.ProfileId.Should().Be(profileId,
            "the token response must include the user's ProfileId so the frontend can send it as a header");
    }

    [Fact]
    public async Task Handle_ShouldReturnNullAccessToken_WhenUserDoesNotExist()
    {
        var mockContext = MockIdentityDbContextFactory.Create(new List<User>());

        var handler = new GetTokenByUsernameAndPasswordRequestHandler(
            mockContext.Object, _hasher.Object, _tokenIssuer.Object);

        var response = await handler.Handle(
            new GetTokenByUsernameAndPasswordRequest
            {
                Username = "missing@example.com",
                Password = "password"
            },
            CancellationToken.None);

        response.AccessToken.Should().BeNull();
        _tokenIssuer.Verify(t => t.Issue(It.IsAny<Guid>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnNullAccessToken_WhenPasswordIsWrong()
    {
        var user = new User
        {
            UserId = Guid.NewGuid(),
            Username = "quinntynebrown@gmail.com",
            PasswordHash = new byte[] { 1, 2, 3 },
            PasswordSalt = new byte[] { 4, 5, 6 }
        };

        var mockContext = MockIdentityDbContextFactory.Create(new List<User> { user });

        _hasher.Setup(h => h.Verify(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(false);

        var handler = new GetTokenByUsernameAndPasswordRequestHandler(
            mockContext.Object, _hasher.Object, _tokenIssuer.Object);

        var response = await handler.Handle(
            new GetTokenByUsernameAndPasswordRequest
            {
                Username = "quinntynebrown@gmail.com",
                Password = "wrong"
            },
            CancellationToken.None);

        response.AccessToken.Should().BeNull();
        _tokenIssuer.Verify(t => t.Issue(It.IsAny<Guid>(), It.IsAny<string>()), Times.Never);
    }
}
