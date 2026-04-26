// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Commitments.Features.GoalProgress;

public class GetGoalProgressLast14Request : IRequest<GetGoalProgressLast14Response>
{
    public Guid GoalId { get; set; }
    public Guid ProfileId { get; set; }
}

public class Last14DayDto
{
    public string Date { get; set; } = string.Empty;
    public int Completed { get; set; }
    public int Target { get; set; }
    public int Percent { get; set; }
}

public class GetGoalProgressLast14Response
{
    public List<Last14DayDto> Points { get; set; } = new();
}

public class GetGoalProgressLast14Handler : IRequestHandler<GetGoalProgressLast14Request, GetGoalProgressLast14Response>
{
    private const int WindowDays = 14;
    private const int DefaultTarget = 30;
    private readonly ICommitmentsDbContext _context;

    public GetGoalProgressLast14Handler(ICommitmentsDbContext context) => _context = context;

    public async Task<GetGoalProgressLast14Response> Handle(GetGoalProgressLast14Request request, CancellationToken cancellationToken)
    {
        var commitment = await _context.Commitments
            .FirstOrDefaultAsync(
                c => c.CommitmentId == request.GoalId && c.ProfileId == request.ProfileId,
                cancellationToken);

        var today = DateTime.UtcNow.Date;
        var earliest = today.AddDays(-(WindowDays - 1));

        var countsByDate = commitment is null
            ? new Dictionary<DateTime, int>()
            : (await _context.Activities
                .Where(a => a.ProfileId == request.ProfileId
                            && a.BehaviourId == commitment.BehaviourId
                            && a.PerformedOn >= earliest)
                .ToListAsync(cancellationToken))
                .GroupBy(a => a.PerformedOn.Date)
                .ToDictionary(g => g.Key, g => g.Count());

        var points = new List<Last14DayDto>(WindowDays);
        for (var i = 0; i < WindowDays; i++)
        {
            var date = earliest.AddDays(i);
            var completed = countsByDate.TryGetValue(date, out var c) ? c : 0;
            points.Add(new Last14DayDto
            {
                Date = date.ToString("yyyy-MM-dd"),
                Completed = completed,
                Target = DefaultTarget,
                Percent = (int)Math.Round((double)completed / DefaultTarget * 100)
            });
        }

        return new GetGoalProgressLast14Response { Points = points };
    }
}
