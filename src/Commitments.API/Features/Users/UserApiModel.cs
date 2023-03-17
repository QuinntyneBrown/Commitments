using Commitments.Core.Entities;


namespace Commitments.Api.Features.Users;

public class UserDto
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public static UserDto FromUser(User user)
        => new UserDto
        {
            UserId = user.UserId,
            Username = user.Username
        };
}
