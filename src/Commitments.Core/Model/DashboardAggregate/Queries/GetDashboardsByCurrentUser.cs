// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Http;

namespace Commitments.Core.Model.DashboardAggregate.Queries;

public class GetDashboardsByCurrentUserRequest : IRequest<GetDashboardsByCurrentUserResponse> { }

public class GetDashboardsByCurrentUserResponse : ResponseBase
{
    public required List<DashboardDto> Dashboards { get; set; }
}


public class GetDashboardsByCurrentUserRequestHandler : IRequestHandler<GetDashboardsByCurrentUserRequest, GetDashboardsByCurrentUserResponse>
{
    private readonly ILogger<GetDashboardsByCurrentUserRequestHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICommitmentsDbContext _context;

    public GetDashboardsByCurrentUserRequestHandler(
        ILogger<GetDashboardsByCurrentUserRequestHandler> logger,
        ICommitmentsDbContext context,
        IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public async Task<GetDashboardsByCurrentUserResponse> Handle(GetDashboardsByCurrentUserRequest request, CancellationToken cancellationToken)
    {

        _logger.LogInformation("Get Dashboards for current user");

        return new()
        {
            Dashboards = await _context.Users
            .Include(x => x.Dashboards)
            .ThenInclude(x => x.DashboardCards)
            .Where(x => x.Username == _httpContextAccessor!.HttpContext!.User.Identity!.Name)
            .SelectMany(x => x.Dashboards.Select(x => x.ToDto()))
            .ToListAsync()
        };
    }
}