using Commitments.Shared;
using Identity.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Identity.Api.Features.User;

public class GetUsersRequest : IRequest<GetUsersResponse> { }

public class GetUsersResponse : ResponseBase
{
    public required List<UserDto> Users { get; set; }
}

public class GetUsersRequestHandler : IRequestHandler<GetUsersRequest, GetUsersResponse>
{
    private readonly IIdentityDbContext _context;

    public GetUsersRequestHandler(IIdentityDbContext context) => _context = context;

    public async Task<GetUsersResponse> Handle(GetUsersRequest request, CancellationToken cancellationToken)
    {
        return new GetUsersResponse
        {
            Users = await _context.Users.ToDtosAsync(cancellationToken)
        };
    }
}
