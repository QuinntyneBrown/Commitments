// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Commitments.Features.GoalProgress;

public class GetGoalTrendRequest : IRequest<GetGoalTrendResponse>
{
    public Guid GoalId { get; set; }
    public Guid ProfileId { get; set; }
    public int? WindowDays { get; set; }
    public DateTimeOffset? AsOf { get; set; }
}

public class GoalTrendPointDto
{
    public string Date { get; set; } = string.Empty;
    public int Completed { get; set; }
    public int Target { get; set; }
    public int Percentage { get; set; }
}

public class GetGoalTrendResponse
{
    public Guid GoalId { get; set; }
    public string Mode { get; set; } = "live";
    public string AsOf { get; set; } = string.Empty;
    public int WindowDays { get; set; }
    public List<GoalTrendPointDto> Points { get; set; } = new();
    public int CurrentPercentage { get; set; }
    public int PeakPercentage { get; set; }
    public int LowPercentage { get; set; }
    public string DeltaLabel { get; set; } = string.Empty;
}

public class GetGoalTrendHandler : IRequestHandler<GetGoalTrendRequest, GetGoalTrendResponse>
{
    private const int DefaultWindowDays = 30;
    private const int MaxWindowDays = 365;
    private const int DefaultTarget = 30;

    private readonly ICommitmentsDbContext _context;

    public GetGoalTrendHandler(ICommitmentsDbContext context) => _context = context;

    public async Task<GetGoalTrendResponse> Handle(GetGoalTrendRequest request, CancellationToken cancellationToken)
    {
        var windowDays = ClampWindow(request.WindowDays ?? DefaultWindowDays);
        var asOf = request.AsOf ?? DateTimeOffset.UtcNow;
        if (asOf > DateTimeOffset.UtcNow) asOf = DateTimeOffset.UtcNow;

        var commitment = await _context.Commitments
            .FirstOrDefaultAsync(
                c => c.CommitmentId == request.GoalId && c.ProfileId == request.ProfileId,
                cancellationToken);

        var endDate = asOf.UtcDateTime.Date;
        var startDate = endDate.AddDays(-(windowDays - 1));

        var countsByDate = commitment is null
            ? new Dictionary<DateTime, int>()
            : (await _context.Activities
                .Where(a => a.ProfileId == request.ProfileId
                            && a.BehaviourId == commitment.BehaviourId
                            && a.PerformedOn >= startDate
                            && a.PerformedOn <= endDate)
                .ToListAsync(cancellationToken))
                .GroupBy(a => a.PerformedOn.Date)
                .ToDictionary(g => g.Key, g => g.Count());

        var points = new List<GoalTrendPointDto>(windowDays);
        for (var i = 0; i < windowDays; i++)
        {
            var date = startDate.AddDays(i);
            var completed = countsByDate.TryGetValue(date, out var c) ? c : 0;
            points.Add(new GoalTrendPointDto
            {
                Date = date.ToString("yyyy-MM-dd"),
                Completed = completed,
                Target = DefaultTarget,
                Percentage = (int)Math.Round((double)completed / DefaultTarget * 100)
            });
        }

        var current = points[^1].Percentage;
        var peak = points.Max(p => p.Percentage);
        var low = points.Min(p => p.Percentage);
        var prior = points.Count >= 14
            ? (int)Math.Round(points.Take(points.Count - 14).Select(p => (double)p.Percentage).DefaultIfEmpty(0).Average())
            : 0;
        var delta = current - prior;
        var sign = delta > 0 ? "+" : delta < 0 ? "" : "±";

        return new GetGoalTrendResponse
        {
            GoalId = request.GoalId,
            Mode = request.AsOf.HasValue ? "review" : "live",
            AsOf = endDate.ToString("yyyy-MM-dd"),
            WindowDays = windowDays,
            Points = points,
            CurrentPercentage = current,
            PeakPercentage = peak,
            LowPercentage = low,
            DeltaLabel = $"{sign}{delta}% vs prior 14d"
        };
    }

    private static int ClampWindow(int value)
    {
        if (value < 1) return 1;
        if (value > MaxWindowDays) return MaxWindowDays;
        return value;
    }
}
