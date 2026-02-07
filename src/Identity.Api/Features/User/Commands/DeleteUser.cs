using Commitments.Shared;
using Identity.Core;
using MediatR;

namespace Identity.Api.Features.User;

public class DeleteUserRequest : IRequest<DeleteUserResponse>
{
    public Guid UserId { get; set; }
}

public class DeleteUserResponse : ResponseBase { }

public class DeleteUserRequestHandler : IRequestHandler<DeleteUserRequest, DeleteUserResponse>
{
    private readonly IIdentityDbContext _context;

    public DeleteUserRequestHandler(IIdentityDbContext context) => _context = context;

    public async Task<DeleteUserResponse> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync(request.UserId);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);
        }
        return new DeleteUserResponse();
    }
}
