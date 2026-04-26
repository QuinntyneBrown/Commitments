// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Commitments.Features.GoalProgress;

public class GetGoalProgressRequest : IRequest<GetGoalProgressResponse>
{
    public Guid GoalId { get; set; }
    public Guid ProfileId { get; set; }
}

public class GetGoalProgressResponse
{
    public Guid GoalId { get; set; }
    public int Target { get; set; }
    public int Count { get; set; }
    public DateTimeOffset AsOf { get; set; }
}

public class GetGoalProgressHandler : IRequestHandler<GetGoalProgressRequest, GetGoalProgressResponse>
{
    private const int DefaultTarget = 30;
    private readonly ICommitmentsDbContext _context;

    public GetGoalProgressHandler(ICommitmentsDbContext context) => _context = context;

    public async Task<GetGoalProgressResponse> Handle(GetGoalProgressRequest request, CancellationToken cancellationToken)
    {
        var commitment = await _context.Commitments
            .FirstOrDefaultAsync(
                c => c.CommitmentId == request.GoalId && c.ProfileId == request.ProfileId,
                cancellationToken);

        var count = commitment is null
            ? 0
            : await _context.Activities
                .Where(a => a.ProfileId == request.ProfileId && a.BehaviourId == commitment.BehaviourId)
                .CountAsync(cancellationToken);

        return new GetGoalProgressResponse
        {
            GoalId = request.GoalId,
            Target = DefaultTarget,
            Count = count,
            AsOf = DateTimeOffset.UtcNow
        };
    }
}
