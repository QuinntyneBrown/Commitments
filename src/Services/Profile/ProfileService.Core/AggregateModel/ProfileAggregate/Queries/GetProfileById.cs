// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace ProfileService.Core.AggregateModel.ProfileAggregate.Queries;

public class GetProfileByIdRequest : IRequest<GetProfileByIdResponse>
{
    public Guid ProfileId { get; set; }
}


public class GetProfileByIdResponse
{
    public required ProfileDto Profile { get; set; }
}


public class GetProfileByIdRequestHandler : IRequestHandler<GetProfileByIdRequest, GetProfileByIdResponse>
{
    private readonly IProfileServiceDbContext _context;

    private readonly ILogger<GetProfileByIdRequestHandler> _logger;

    public GetProfileByIdRequestHandler(ILogger<GetProfileByIdRequestHandler> logger, IProfileServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetProfileByIdResponse> Handle(GetProfileByIdRequest request, CancellationToken cancellationToken)
    {
        return new()
        {
            Profile = (await _context.Profiles.AsNoTracking().SingleOrDefaultAsync(x => x.ProfileId == request.ProfileId)).ToDto()
        };

    }

}