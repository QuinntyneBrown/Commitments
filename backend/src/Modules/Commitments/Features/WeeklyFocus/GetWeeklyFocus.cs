// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Commitments.Features.WeeklyFocus;

public class GetWeeklyFocusRequest : IRequest<GetWeeklyFocusResponse>
{
    public Guid ProfileId { get; set; }
    public DateTimeOffset? AsOf { get; set; }
}

public class WeeklyFocusItem
{
    public string Name { get; set; } = string.Empty;
    public string SupportingMetric { get; set; } = string.Empty;
    public int Rank { get; set; }
}

public class GetWeeklyFocusResponse
{
    public string Mode { get; set; } = "live";
    public DateTimeOffset AsOf { get; set; }
    public string WeekStart { get; set; } = string.Empty;
    public string WeekEnd { get; set; } = string.Empty;
    public List<WeeklyFocusItem> FocusAreas { get; set; } = new();
}

public class GetWeeklyFocusHandler : IRequestHandler<GetWeeklyFocusRequest, GetWeeklyFocusResponse>
{
    private const int TopFocusAreas = 3;
    private readonly ICommitmentsDbContext _context;

    public GetWeeklyFocusHandler(ICommitmentsDbContext context) => _context = context;

    public async Task<GetWeeklyFocusResponse> Handle(GetWeeklyFocusRequest request, CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;
        var mode = request.AsOf is null ? "live" : "review";
        var effectiveAsOf = request.AsOf is null || request.AsOf > now ? now : request.AsOf.Value;

        var asOfUtc = effectiveAsOf.UtcDateTime;
        var daysSinceMonday = ((int)asOfUtc.DayOfWeek - 1 + 7) % 7;
        var weekStart = asOfUtc.Date.AddDays(-daysSinceMonday);
        var weekEnd = weekStart.AddDays(6);

        var topBehaviours = await _context.Activities
            .Where(a => a.ProfileId == request.ProfileId
                && a.PerformedOn >= weekStart
                && a.PerformedOn <= asOfUtc)
            .GroupBy(a => new { a.BehaviourId, a.Behaviour.Name })
            .Select(g => new { g.Key.Name, Count = g.Count() })
            .OrderByDescending(g => g.Count)
            .Take(TopFocusAreas)
            .ToListAsync(cancellationToken);

        var focusAreas = topBehaviours
            .Select((g, i) => new WeeklyFocusItem
            {
                Name = g.Name,
                SupportingMetric = $"{g.Count} sessions logged",
                Rank = i + 1
            })
            .ToList();

        return new GetWeeklyFocusResponse
        {
            Mode = mode,
            AsOf = effectiveAsOf,
            WeekStart = weekStart.ToString("yyyy-MM-dd"),
            WeekEnd = weekEnd.ToString("yyyy-MM-dd"),
            FocusAreas = focusAreas
        };
    }
}
