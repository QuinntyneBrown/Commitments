using FluentValidation;
using Identity.Data;
using Identity.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Identity.Features.User;

public class GetTokenByUsernameAndPasswordRequest : IRequest<GetTokenByUsernameAndPasswordResponse>
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class GetTokenByUsernameAndPasswordResponse
{
    public string? AccessToken { get; set; }
    public Guid? ProfileId { get; set; }
}

public class GetTokenByUsernameAndPasswordRequestValidator : AbstractValidator<GetTokenByUsernameAndPasswordRequest>
{
    public GetTokenByUsernameAndPasswordRequestValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}

public class GetTokenByUsernameAndPasswordRequestHandler
    : IRequestHandler<GetTokenByUsernameAndPasswordRequest, GetTokenByUsernameAndPasswordResponse>
{
    private readonly IIdentityDbContext _context;
    private readonly IPasswordHasher _hasher;
    private readonly IAccessTokenIssuer _tokenIssuer;

    public GetTokenByUsernameAndPasswordRequestHandler(
        IIdentityDbContext context,
        IPasswordHasher hasher,
        IAccessTokenIssuer tokenIssuer)
    {
        _context = context;
        _hasher = hasher;
        _tokenIssuer = tokenIssuer;
    }

    public async Task<GetTokenByUsernameAndPasswordResponse> Handle(
        GetTokenByUsernameAndPasswordRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .SingleOrDefaultAsync(u => u.Username == request.Username, cancellationToken);

        if (user is null || user.PasswordHash is null || user.PasswordSalt is null)
            return new GetTokenByUsernameAndPasswordResponse();

        if (!_hasher.Verify(request.Password, user.PasswordSalt, user.PasswordHash))
            return new GetTokenByUsernameAndPasswordResponse();

        var profile = await _context.Profiles
            .Where(p => p.UserId == user.UserId)
            .OrderBy(p => p.CreatedOn)
            .FirstOrDefaultAsync(cancellationToken);

        return new GetTokenByUsernameAndPasswordResponse
        {
            AccessToken = _tokenIssuer.Issue(user.UserId, user.Username),
            ProfileId = profile?.ProfileId
        };
    }
}
