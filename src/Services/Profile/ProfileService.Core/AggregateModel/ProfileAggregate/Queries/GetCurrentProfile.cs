// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Http;

namespace ProfileService.Core.AggregateModel.ProfileAggregate.Queries;

public class GetCurremtProfileRequest : IRequest<GetCurremtProfileResponse> { }

public class GetCurremtProfileResponse
{
    public required ProfileDto Profile { get; set; }
}


public class GetCurremtProfileRequestHandler : IRequestHandler<GetCurremtProfileRequest, GetCurremtProfileResponse>
{
    private readonly IProfileServiceDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<GetCurremtProfileRequestHandler> _logger;

    public GetCurremtProfileRequestHandler(
        ILogger<GetCurremtProfileRequestHandler> logger,
        IProfileServiceDbContext context,
        IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public async Task<GetCurremtProfileResponse> Handle(GetCurremtProfileRequest request, CancellationToken cancellationToken)
    {
        var username = _httpContextAccessor.HttpContext!.User.Identity!.Name;

        var profile = await _context.Profiles.SingleAsync(x => x.Username == username);

        return new()
        {
            Profile = profile.ToDto()
        };

    }

}