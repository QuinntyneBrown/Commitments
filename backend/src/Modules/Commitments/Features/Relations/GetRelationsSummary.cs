// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Commitments.Features.Relations;

public class GetRelationsSummaryRequest : IRequest<GetRelationsSummaryResponse>
{
    public Guid ProfileId { get; set; }
    public DateTimeOffset? AsOf { get; set; }
    public int Top { get; set; } = 3;
}

public class RelationsSummaryItem
{
    public Guid BehaviourTypeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Count { get; set; }
    public int Percentage { get; set; }
}

public class GetRelationsSummaryResponse
{
    public string Mode { get; set; } = "live";
    public DateTimeOffset AsOf { get; set; }
    public int TotalCommitments { get; set; }
    public List<RelationsSummaryItem> Relations { get; set; } = new();
}

public class GetRelationsSummaryHandler : IRequestHandler<GetRelationsSummaryRequest, GetRelationsSummaryResponse>
{
    private const int MaxTop = 10;
    private readonly ICommitmentsDbContext _context;

    public GetRelationsSummaryHandler(ICommitmentsDbContext context) => _context = context;

    public async Task<GetRelationsSummaryResponse> Handle(GetRelationsSummaryRequest request, CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;
        var mode = request.AsOf is null ? "live" : "review";
        var effectiveAsOf = request.AsOf is null || request.AsOf > now ? now : request.AsOf.Value;
        var asOfUtc = effectiveAsOf.UtcDateTime;
        var top = Math.Clamp(request.Top, 1, MaxTop);

        var groups = await _context.Commitments
            .Where(c => c.ProfileId == request.ProfileId && c.CreatedOn <= asOfUtc)
            .GroupBy(c => new { c.Behaviour.BehaviourType.BehaviourTypeId, c.Behaviour.BehaviourType.Name })
            .Select(g => new { g.Key.BehaviourTypeId, g.Key.Name, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var total = groups.Sum(g => g.Count);
        var ordered = groups.OrderByDescending(g => g.Count).Take(top).ToList();

        var relations = total > 0
            ? RoundPercentagesToSum100(ordered.Select(g => (g.BehaviourTypeId, g.Name, g.Count)).ToList(), total)
            : new List<RelationsSummaryItem>();

        return new GetRelationsSummaryResponse
        {
            Mode = mode,
            AsOf = effectiveAsOf,
            TotalCommitments = total,
            Relations = relations
        };
    }

    // Largest-remainder method: floor each percentage, then add 1 to those with largest fractional parts
    // until the sum equals 100. Guarantees percentages sum to exactly 100 (when there is at least one row).
    private static List<RelationsSummaryItem> RoundPercentagesToSum100(
        List<(Guid Id, string Name, int Count)> rows, int total)
    {
        var raw = rows.Select(r => (r.Id, r.Name, r.Count, Exact: r.Count * 100.0 / total)).ToList();
        var floored = raw.Select(r => (r.Id, r.Name, r.Count, Floor: (int)Math.Floor(r.Exact), Frac: r.Exact - Math.Floor(r.Exact))).ToList();
        var leftover = 100 - floored.Sum(f => f.Floor);

        var winnersIds = floored
            .Select((f, idx) => (idx, f.Frac))
            .OrderByDescending(x => x.Frac)
            .Take(leftover)
            .Select(x => x.idx)
            .ToHashSet();

        return floored.Select((f, idx) => new RelationsSummaryItem
        {
            BehaviourTypeId = f.Id,
            Name = f.Name,
            Count = f.Count,
            Percentage = f.Floor + (winnersIds.Contains(idx) ? 1 : 0)
        }).ToList();
    }
}
