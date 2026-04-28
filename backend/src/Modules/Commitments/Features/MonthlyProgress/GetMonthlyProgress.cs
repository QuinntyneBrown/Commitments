// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Commitments.Features.MonthlyProgress;

public class GetMonthlyProgressRequest : IRequest<GetMonthlyProgressResponse>
{
    public Guid ProfileId { get; set; }
    public DateTimeOffset? AsOf { get; set; }
}

public class MonthlyProgressBucket
{
    public string WeekStart { get; set; } = string.Empty;
    public string WeekEnd { get; set; } = string.Empty;
    public int Completed { get; set; }
    public int Target { get; set; }
    public int Percentage { get; set; }
}

public class GetMonthlyProgressResponse
{
    public string Mode { get; set; } = "live";
    public DateTimeOffset AsOf { get; set; }
    public int WindowDays { get; set; } = 30;
    public List<MonthlyProgressBucket> Buckets { get; set; } = new();
    public bool IsEmpty { get; set; } = true;
}

public class GetMonthlyProgressHandler : IRequestHandler<GetMonthlyProgressRequest, GetMonthlyProgressResponse>
{
    private const string DailyFrequencyTypeName = "per day";
    private const int BucketCount = 4;
    private const int BucketDays = 7;
    private readonly ICommitmentsDbContext _context;

    public GetMonthlyProgressHandler(ICommitmentsDbContext context) => _context = context;

    public async Task<GetMonthlyProgressResponse> Handle(GetMonthlyProgressRequest request, CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;
        var mode = request.AsOf is null ? "live" : "review";
        var effectiveAsOf = request.AsOf is null || request.AsOf > now ? now : request.AsOf.Value;
        var asOfUtc = effectiveAsOf.UtcDateTime;
        var windowStart = asOfUtc.AddDays(-BucketCount * BucketDays);

        var dailyBehaviourIds = await _context.Commitments
            .Where(c => c.ProfileId == request.ProfileId
                && c.CommitmentFrequencies.Any(f => f.Frequency.FrequencyType.Name == DailyFrequencyTypeName))
            .Select(c => c.BehaviourId)
            .ToListAsync(cancellationToken);

        var activityDates = await _context.Activities
            .Where(a => a.ProfileId == request.ProfileId
                && a.PerformedOn >= windowStart
                && a.PerformedOn < asOfUtc
                && dailyBehaviourIds.Contains(a.BehaviourId))
            .Select(a => a.PerformedOn)
            .ToListAsync(cancellationToken);

        var dailyCommitmentCount = dailyBehaviourIds.Count;
        var bucketTarget = BucketDays * dailyCommitmentCount;
        var buckets = new List<MonthlyProgressBucket>(BucketCount);

        for (var i = 0; i < BucketCount; i++)
        {
            var bucketStart = asOfUtc.AddDays(-(BucketCount - i) * BucketDays);
            var bucketEnd = asOfUtc.AddDays(-(BucketCount - i - 1) * BucketDays);
            var completed = activityDates.Count(p => p >= bucketStart && p < bucketEnd);
            var percentage = bucketTarget > 0
                ? Math.Clamp((int)Math.Round((double)completed / bucketTarget * 100), 0, 100)
                : 0;

            buckets.Add(new MonthlyProgressBucket
            {
                WeekStart = bucketStart.ToString("yyyy-MM-dd"),
                WeekEnd = bucketEnd.ToString("yyyy-MM-dd"),
                Completed = completed,
                Target = bucketTarget,
                Percentage = percentage
            });
        }

        return new GetMonthlyProgressResponse
        {
            Mode = mode,
            AsOf = effectiveAsOf,
            WindowDays = BucketCount * BucketDays,
            Buckets = buckets,
            IsEmpty = buckets.All(b => b.Completed == 0)
        };
    }
}
