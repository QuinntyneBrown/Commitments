// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace DashboardService.Core.AggregateModel.UserAggregate;

public static class UserExtensions
{
    public static UserDto ToDto(this User User)
    {
        return new UserDto
        {
            UserId = User.UserId,
        };

    }

    public async static Task<List<UserDto>> ToDtosAsync(this IQueryable<User> Users, CancellationToken cancellationToken)
    {
        return await Users.Select(x => x.ToDto()).ToListAsync(cancellationToken);
    }

}


