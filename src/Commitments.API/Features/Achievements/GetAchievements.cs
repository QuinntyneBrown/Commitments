// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

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

 public class GetAchievementsRequest : IRequest<GetAchievementsResponse> {
     public int ProfileId { get; set; }
 }

 public class GetAchievementsResponse
 {
     public IEnumerable<AchievementDto> Achievements { get; set; }
 }

 public class GetAchievementsQueryHandler : IRequestHandler<GetAchievementsRequest, GetAchievementsResponse>
 {
     public ICommimentsDbContext _context { get; set; }
     public GetAchievementsQueryHandler(ICommimentsDbContext context) => _context = context;

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

         return new GetAchievementsResponse()
         {
             Achievements = achievements
         };
     }
 }

