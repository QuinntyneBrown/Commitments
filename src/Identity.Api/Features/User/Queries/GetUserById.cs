using Commitments.Shared;
using Identity.Core;
using MediatR;

namespace Identity.Api.Features.User;

public class GetUserByIdRequest : IRequest<GetUserByIdResponse>
{
    public Guid UserId { get; set; }
}

public class GetUserByIdResponse : ResponseBase
{
    public UserDto? User { get; set; }
}

public class GetUserByIdRequestHandler : IRequestHandler<GetUserByIdRequest, GetUserByIdResponse>
{
    private readonly IIdentityDbContext _context;

    public GetUserByIdRequestHandler(IIdentityDbContext context) => _context = context;

    public async Task<GetUserByIdResponse> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync(request.UserId);
        return new GetUserByIdResponse { User = user?.ToDto() };
    }
}
