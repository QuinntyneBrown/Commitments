// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Commitments.Features.GoalProgress;

public class GetGoalProgressAtRequest : IRequest<GetGoalProgressResponse>
{
    public Guid GoalId { get; set; }
    public Guid ProfileId { get; set; }
    public DateTimeOffset AsOf { get; set; }
}

public class GetGoalProgressAtHandler : IRequestHandler<GetGoalProgressAtRequest, GetGoalProgressResponse>
{
    private const int DefaultTarget = 30;
    private readonly ICommitmentsDbContext _context;

    public GetGoalProgressAtHandler(ICommitmentsDbContext context) => _context = context;

    public async Task<GetGoalProgressResponse> Handle(GetGoalProgressAtRequest request, CancellationToken cancellationToken)
    {
        var asOf = request.AsOf > DateTimeOffset.UtcNow ? DateTimeOffset.UtcNow : request.AsOf;

        var commitment = await _context.Commitments
            .FirstOrDefaultAsync(
                c => c.CommitmentId == request.GoalId && c.ProfileId == request.ProfileId,
                cancellationToken);

        var count = commitment is null
            ? 0
            : await _context.Activities
                .Where(a => a.ProfileId == request.ProfileId
                            && a.BehaviourId == commitment.BehaviourId
                            && a.PerformedOn <= asOf.UtcDateTime)
                .CountAsync(cancellationToken);

        return new GetGoalProgressResponse
        {
            GoalId = request.GoalId,
            Target = DefaultTarget,
            Count = count,
            AsOf = asOf
        };
    }
}
