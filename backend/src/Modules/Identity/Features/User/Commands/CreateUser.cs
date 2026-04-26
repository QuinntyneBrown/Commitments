using Commitments.Shared;
using Identity.Data;
using MediatR;

namespace Identity.Features.User;

public class CreateUserRequest : IRequest<CreateUserResponse>
{
    public UserDto User { get; set; }
}

public class CreateUserResponse : ResponseBase
{
    public UserDto User { get; set; }
}

public class CreateUserRequestHandler : IRequestHandler<CreateUserRequest, CreateUserResponse>
{
    private readonly IIdentityDbContext _context;

    public CreateUserRequestHandler(IIdentityDbContext context) => _context = context;

    public async Task<CreateUserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var user = new Identity.Domain.UserAggregate.User
        {
            Username = request.User.Username,
            FirstName = request.User.FirstName,
            LastName = request.User.LastName,
            Email = request.User.Email
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateUserResponse { User = user.ToDto() };
    }
}
