using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Commitments.Api.Features.Commitments;
using System;


namespace Commitments.Api.Features.Achievements;

 public class GetAchievementsQueryRequest : IRequest<GetAchievementsQueryResponse> {
     public int ProfileId { get; set; }
 }

 public class GetAchievementsQueryResponse
 {
     public IEnumerable<AchievementDto> Achievements { get; set; }
 }

 public class GetAchievementsQueryHandler : IRequestHandler<GetAchievementsQueryRequest, GetAchievementsQueryResponse>
 {
     public ICommimentsDbContext _context { get; set; }
     public GetAchievementsQueryHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetAchievementsQueryResponse> Handle(GetAchievementsQueryRequest request, CancellationToken cancellationToken)
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

         foreach(var dailyCommitment in dailyCommitments)
         {
             var activity = _context.Activities.FirstOrDefault(x => x.ProfileId == request.ProfileId 
             && x.BehaviourId == dailyCommitment.BehaviourId
             && x.PerformedOn.Date == DateTime.Now.Date);

             if(activity != null)
                 achievements.Add(new AchievementDto()
                 {
                     Commitment = CommitmentDto.FromCommitment(dailyCommitment)
                 });
         }

         return new GetAchievementsQueryResponse()
         {
             Achievements = achievements
         };
     }
 }
