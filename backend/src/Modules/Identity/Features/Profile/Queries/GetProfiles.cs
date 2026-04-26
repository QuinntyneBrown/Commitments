using Commitments.Shared;
using Identity.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Identity.Features.Profile;

public class GetProfilesRequest : IRequest<GetProfilesResponse> { }

public class GetProfilesResponse : ResponseBase
{
    public required List<ProfileDto> Profiles { get; set; }
}

public class GetProfilesRequestHandler : IRequestHandler<GetProfilesRequest, GetProfilesResponse>
{
    private readonly IIdentityDbContext _context;

    public GetProfilesRequestHandler(IIdentityDbContext context) => _context = context;

    public async Task<GetProfilesResponse> Handle(GetProfilesRequest request, CancellationToken cancellationToken)
    {
        return new GetProfilesResponse
        {
            Profiles = await _context.Profiles.ToDtosAsync(cancellationToken)
        };
    }
}
