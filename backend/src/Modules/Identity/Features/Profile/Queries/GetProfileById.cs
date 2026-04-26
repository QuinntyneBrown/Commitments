using Commitments.Shared;
using Identity.Data;
using MediatR;

namespace Identity.Features.Profile;

public class GetProfileByIdRequest : IRequest<GetProfileByIdResponse>
{
    public Guid ProfileId { get; set; }
}

public class GetProfileByIdResponse : ResponseBase
{
    public ProfileDto? Profile { get; set; }
}

public class GetProfileByIdRequestHandler : IRequestHandler<GetProfileByIdRequest, GetProfileByIdResponse>
{
    private readonly IIdentityDbContext _context;

    public GetProfileByIdRequestHandler(IIdentityDbContext context) => _context = context;

    public async Task<GetProfileByIdResponse> Handle(GetProfileByIdRequest request, CancellationToken cancellationToken)
    {
        var profile = await _context.Profiles.FindAsync(request.ProfileId);
        return new GetProfileByIdResponse { Profile = profile?.ToDto() };
    }
}
