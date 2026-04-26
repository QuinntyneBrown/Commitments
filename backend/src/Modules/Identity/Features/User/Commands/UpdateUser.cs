using Commitments.Shared;
using Identity.Data;
using MediatR;

namespace Identity.Features.User;

public class UpdateUserRequest : IRequest<UpdateUserResponse>
{
    public UserDto User { get; set; }
}

public class UpdateUserResponse : ResponseBase
{
    public UserDto User { get; set; }
}

public class UpdateUserRequestHandler : IRequestHandler<UpdateUserRequest, UpdateUserResponse>
{
    private readonly IIdentityDbContext _context;

    public UpdateUserRequestHandler(IIdentityDbContext context) => _context = context;

    public async Task<UpdateUserResponse> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync(request.User.UserId);
        if (user == null) return new UpdateUserResponse();

        user.Username = request.User.Username;
        user.FirstName = request.User.FirstName;
        user.LastName = request.User.LastName;
        user.Email = request.User.Email;

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateUserResponse { User = user.ToDto() };
    }
}
