using Identity.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Identity.Features.Profile;

public class GetCurrentProfileRequest : IRequest<GetCurrentProfileResponse>
{
    public Guid UserId { get; set; }
}

public class GetCurrentProfileResponse
{
    public ProfileDto? Profile { get; set; }
}

public class GetCurrentProfileRequestHandler
    : IRequestHandler<GetCurrentProfileRequest, GetCurrentProfileResponse>
{
    private readonly IIdentityDbContext _context;

    public GetCurrentProfileRequestHandler(IIdentityDbContext context) => _context = context;

    public async Task<GetCurrentProfileResponse> Handle(
        GetCurrentProfileRequest request,
        CancellationToken cancellationToken)
    {
        var profile = await _context.Profiles
            .Where(p => p.UserId == request.UserId)
            .OrderBy(p => p.CreatedOn)
            .FirstOrDefaultAsync(cancellationToken);

        return new GetCurrentProfileResponse { Profile = profile?.ToDto() };
    }
}
