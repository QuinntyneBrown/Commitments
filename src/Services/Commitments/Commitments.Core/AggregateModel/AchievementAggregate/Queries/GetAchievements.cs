// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel.CommitmentAggregate;
using Commitments.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Commitments.Core.AggregateModel.AchievementAggregate.Queries;

public class GetAchievementsRequest : IRequest<GetAchievementsResponse>
{
    public Guid ProfileId { get; set; }
}

public class GetAchievementsResponse
{
    public IEnumerable<AchievementDto> Achievements { get; set; }
}

public class GetAchievementsQueryHandler : IRequestHandler<GetAchievementsRequest, GetAchievementsResponse>
{
    public ICommitmentsDbContext _context { get; set; }
    public GetAchievementsQueryHandler(ICommitmentsDbContext context) => _context = context;

    public async Task<GetAchievementsResponse> Handle(GetAchievementsRequest request, CancellationToken cancellationToken)
    {
        var achievements = new List<AchievementDto>();
        var dailyCommitments = await _context.Commitments
            .Include(x => x.Behaviour)
            .Include("Behaviour.BehaviourType")
            .Include(x => x.CommitmentFrequencies)
            .Include("CommitmentFrequencies.Frequency")
            .Include("CommitmentFrequencies.Frequency.FrequencyType")
            .Where(x => x.ProfileId == request.ProfileId && x.CommitmentFrequencies.Any(f => f.Frequency.FrequencyType.Name == "per day"))
            .ToListAsync();

        foreach (var dailyCommitment in dailyCommitments)
        {
            var activity = _context.Activities.FirstOrDefault(x => x.ProfileId == request.ProfileId
            && x.BehaviourId == dailyCommitment.BehaviourId
            && x.PerformedOn.Date == DateTime.Now.Date);

            if (activity != null)
                achievements.Add(new AchievementDto()
                {
                    Commitment = CommitmentDto.FromCommitment(dailyCommitment)
                });
        }

        return new GetAchievementsResponse()
        {
            Achievements = achievements
        };
    }
}

