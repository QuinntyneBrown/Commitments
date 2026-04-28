using Identity.Data;
using MediatR;

namespace Identity.Features.Profile;

public class SaveAvatarUrlRequest : IRequest<SaveAvatarUrlResponse>
{
    public Guid ProfileId { get; set; }
    public string? AvatarUrl { get; set; }
}

public class SaveAvatarUrlResponse
{
    public Guid ProfileId { get; set; }
}

public class SaveAvatarUrlRequestHandler : IRequestHandler<SaveAvatarUrlRequest, SaveAvatarUrlResponse>
{
    private readonly IIdentityDbContext _context;

    public SaveAvatarUrlRequestHandler(IIdentityDbContext context) => _context = context;

    public async Task<SaveAvatarUrlResponse> Handle(SaveAvatarUrlRequest request, CancellationToken cancellationToken)
    {
        var profile = await _context.Profiles.FindAsync(request.ProfileId);
        if (profile is null) return new SaveAvatarUrlResponse { ProfileId = request.ProfileId };

        profile.AvatarUrl = request.AvatarUrl;
        await _context.SaveChangesAsync(cancellationToken);

        return new SaveAvatarUrlResponse { ProfileId = profile.ProfileId };
    }
}
