using Identity.Core.Model.UserAggregate;
using Microsoft.EntityFrameworkCore;

namespace Identity.Api.Features.User;

public static class UserExtensions
{
    public static UserDto ToDto(this Identity.Core.Model.UserAggregate.User user)
    {
        return new UserDto
        {
            UserId = user.UserId,
            Username = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
    }

    public static async Task<List<UserDto>> ToDtosAsync(this IQueryable<Identity.Core.Model.UserAggregate.User> users, CancellationToken cancellationToken)
    {
        return await users.Select(x => x.ToDto()).ToListAsync(cancellationToken);
    }
}
