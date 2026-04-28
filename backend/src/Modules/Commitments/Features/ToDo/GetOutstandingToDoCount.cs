// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Domain.ToDoAggregate;

namespace Commitments.Features.ToDo;

public class GetOutstandingToDoCountRequest : IRequest<GetOutstandingToDoCountResponse>
{
    public Guid ProfileId { get; set; }
    public DateTimeOffset? AsOf { get; set; }
    public bool IncludeToday { get; set; }
}

public class GetOutstandingToDoCountResponse
{
    public string Mode { get; set; } = "live";
    public DateTimeOffset AsOf { get; set; }
    public int Count { get; set; }
    public int? TodayCount { get; set; }
}

public class GetOutstandingToDoCountHandler : IRequestHandler<GetOutstandingToDoCountRequest, GetOutstandingToDoCountResponse>
{
    private readonly ICommitmentsDbContext _context;

    public GetOutstandingToDoCountHandler(ICommitmentsDbContext context) => _context = context;

    public async Task<GetOutstandingToDoCountResponse> Handle(GetOutstandingToDoCountRequest request, CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;
        var mode = request.AsOf is null ? "live" : "review";
        var effectiveAsOf = request.AsOf is null || request.AsOf > now ? now : request.AsOf.Value;

        var count = await CountOutstanding(request.ProfileId, effectiveAsOf.UtcDateTime, cancellationToken);
        int? todayCount = request.IncludeToday
            ? await CountOutstanding(request.ProfileId, now.UtcDateTime, cancellationToken)
            : null;

        return new GetOutstandingToDoCountResponse
        {
            Mode = mode,
            AsOf = effectiveAsOf,
            Count = count,
            TodayCount = todayCount
        };
    }

    private Task<int> CountOutstanding(Guid profileId, DateTime atUtc, CancellationToken cancellationToken) =>
        _context.Todos
            .Where(t => t.ProfileId == profileId
                && t.CreatedOn <= atUtc
                && (t.CompletedOn == null || t.CompletedOn > atUtc)
                && (t.DeletedOn == null || t.DeletedOn > atUtc))
            .CountAsync(cancellationToken);
}
