// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Commitments.Features.Commitment;

public class GetDailyResultsRequest : IRequest<GetDailyResultsResponse>
{
    public Guid ProfileId { get; set; }
    public DateTimeOffset? AsOf { get; set; }
}

public class GetDailyResultsResponse
{
    public string Mode { get; set; } = "live";
    public DateTimeOffset AsOf { get; set; }
    public string Date { get; set; } = string.Empty;
    public int Completed { get; set; }
    public int Total { get; set; }
}

public class GetDailyResultsHandler : IRequestHandler<GetDailyResultsRequest, GetDailyResultsResponse>
{
    private const string DailyFrequencyTypeName = "per day";
    private readonly ICommitmentsDbContext _context;

    public GetDailyResultsHandler(ICommitmentsDbContext context) => _context = context;

    public async Task<GetDailyResultsResponse> Handle(GetDailyResultsRequest request, CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;
        var mode = request.AsOf is null ? "live" : "review";
        var effectiveAsOf = request.AsOf is null || request.AsOf > now ? now : request.AsOf.Value;
        var dayStart = effectiveAsOf.UtcDateTime.Date;

        var dailyCommitments = await _context.Commitments
            .Where(c => c.ProfileId == request.ProfileId
                && c.CommitmentFrequencies.Any(f => f.Frequency.FrequencyType.Name == DailyFrequencyTypeName))
            .Select(c => new { c.CommitmentId, c.BehaviourId })
            .ToListAsync(cancellationToken);

        var dailyBehaviourIds = dailyCommitments.Select(c => c.BehaviourId).ToHashSet();

        var completed = await _context.Activities
            .Where(a => a.ProfileId == request.ProfileId
                && a.PerformedOn >= dayStart
                && a.PerformedOn <= effectiveAsOf.UtcDateTime
                && dailyBehaviourIds.Contains(a.BehaviourId))
            .Select(a => a.BehaviourId)
            .Distinct()
            .CountAsync(cancellationToken);

        return new GetDailyResultsResponse
        {
            Mode = mode,
            AsOf = effectiveAsOf,
            Date = effectiveAsOf.UtcDateTime.ToString("yyyy-MM-dd"),
            Completed = completed,
            Total = dailyCommitments.Count
        };
    }
}
