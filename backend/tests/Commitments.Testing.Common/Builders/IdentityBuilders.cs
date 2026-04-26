using Identity.Core.Model.UserAggregate;
using Identity.Core.Model.ProfileAggregate;

namespace Commitments.Testing.Common.Builders;

public class UserBuilder
{
    private Guid _userId = Guid.NewGuid();
    private string _username = "testuser";
    private string? _firstName = "Test";
    private string? _lastName = "User";
    private string? _email = "test@example.com";

    public UserBuilder WithUserId(Guid id) { _userId = id; return this; }
    public UserBuilder WithUsername(string username) { _username = username; return this; }
    public UserBuilder WithFirstName(string? firstName) { _firstName = firstName; return this; }
    public UserBuilder WithLastName(string? lastName) { _lastName = lastName; return this; }
    public UserBuilder WithEmail(string? email) { _email = email; return this; }

    public User Build() => new User
    {
        UserId = _userId,
        Username = _username,
        FirstName = _firstName,
        LastName = _lastName,
        Email = _email
    };
}

public class ProfileBuilder
{
    private Guid _profileId = Guid.NewGuid();

    public ProfileBuilder WithProfileId(Guid id) { _profileId = id; return this; }

    public Profile Build() => new Profile
    {
        ProfileId = _profileId
    };
}
