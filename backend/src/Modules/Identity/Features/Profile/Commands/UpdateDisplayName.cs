using Identity.Data;
using MediatR;

namespace Identity.Features.Profile;

public class UpdateDisplayNameRequest : IRequest<UpdateDisplayNameResponse>
{
    public Guid ProfileId { get; set; }
    public string? DisplayName { get; set; }
}

public class UpdateDisplayNameResponse
{
    public Guid ProfileId { get; set; }
}

public class UpdateDisplayNameRequestHandler : IRequestHandler<UpdateDisplayNameRequest, UpdateDisplayNameResponse>
{
    private readonly IIdentityDbContext _context;

    public UpdateDisplayNameRequestHandler(IIdentityDbContext context) => _context = context;

    public async Task<UpdateDisplayNameResponse> Handle(UpdateDisplayNameRequest request, CancellationToken cancellationToken)
    {
        var profile = await _context.Profiles.FindAsync(request.ProfileId);
        if (profile is null) return new UpdateDisplayNameResponse { ProfileId = request.ProfileId };

        profile.DisplayName = request.DisplayName;
        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateDisplayNameResponse { ProfileId = profile.ProfileId };
    }
}
