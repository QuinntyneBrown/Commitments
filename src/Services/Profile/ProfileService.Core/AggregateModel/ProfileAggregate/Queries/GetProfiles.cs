// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace ProfileService.Core.AggregateModel.ProfileAggregate.Queries;

public class GetProfilesRequest : IRequest<GetProfilesResponse> { }

public class GetProfilesResponse
{
    public required List<ProfileDto> Profiles { get; set; }
}


public class GetProfilesRequestHandler : IRequestHandler<GetProfilesRequest, GetProfilesResponse>
{
    private readonly IProfileServiceDbContext _context;

    private readonly ILogger<GetProfilesRequestHandler> _logger;

    public GetProfilesRequestHandler(ILogger<GetProfilesRequestHandler> logger, IProfileServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetProfilesResponse> Handle(GetProfilesRequest request, CancellationToken cancellationToken)
    {
        return new()
        {
            Profiles = await _context.Profiles.AsNoTracking().ToDtosAsync(cancellationToken)
        };

    }

}