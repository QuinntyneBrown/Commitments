using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Commitments.API.Features.Commitments;
using System;

namespace Commitments.API.Features.Achievements
{
    public class GetAchievementsQuery
    {
        public class Request : IRequest<Response> {
            public int ProfileId { get; set; }
        }

        public class Response
        {
            public IEnumerable<AchievementApiModel> Achievements { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var achievements = new List<AchievementApiModel>();
                var dailyCommitments = await _context.Commitments
                    .Include(x => x.CommitmentFrequencies)
                    .Include("CommitmentFrequencies.Frequency")
                    .Include("CommitmentFrequencies.Frequency.FrequencyType")
                    .Where(x => x.ProfileId == request.ProfileId && x.CommitmentFrequencies.Any(f => f.Frequency.FrequencyType.Name == "per day"))
                    .ToListAsync();
                
                foreach(var dailyCommitment in dailyCommitments)
                {
                    var activity = _context.Activities.FirstOrDefault(x => x.ProfileId == request.ProfileId 
                    && x.BehaviourId == dailyCommitment.BehaviourId
                    && x.PerformedOn.Date == DateTime.Now.Date);

                    if(activity != null)
                        achievements.Add(new AchievementApiModel()
                        {
                            Commitment = CommitmentApiModel.FromCommitment(dailyCommitment)
                        });
                }

                return new Response()
                {
                    Achievements = achievements
                };
            }
        }
    }
}
