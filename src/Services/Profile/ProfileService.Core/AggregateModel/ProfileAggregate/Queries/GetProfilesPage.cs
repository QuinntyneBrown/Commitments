// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace ProfileService.Core.AggregateModel.ProfileAggregate.Queries;

public class GetProfilesPageRequest : IRequest<GetProfilesPageResponse>
{
    public int PageSize { get; set; }
    public int Index { get; set; }
    public int Length { get; set; }
}


public class GetProfilesPageResponse
{
    public required int Length { get; set; }
    public required List<ProfileDto> Entities { get; set; }
}


public class CreateProfileRequestHandler : IRequestHandler<GetProfilesPageRequest, GetProfilesPageResponse>
{
    private readonly IProfileServiceDbContext _context;

    private readonly ILogger<CreateProfileRequestHandler> _logger;

    public CreateProfileRequestHandler(ILogger<CreateProfileRequestHandler> logger, IProfileServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetProfilesPageResponse> Handle(GetProfilesPageRequest request, CancellationToken cancellationToken)
    {
        var query = from profile in _context.Profiles
                    select profile;

        var length = await _context.Profiles.AsNoTracking().CountAsync();

        var profiles = await query.Page(request.Index, request.PageSize).AsNoTracking()
            .Select(x => x.ToDto()).ToListAsync();

        return new()
        {
            Length = length,
            Entities = profiles
        };

    }

}