using Commitments.API.Features.Achievements;
using Commitments.Core.Entities;
using Commitments.Core.Extensions;
using Commitments.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Features
{
    public class AchievementScenarios: AchievementScenarioBase
    {

        [Fact]
        public async Task ShouldGetAll()
        {
            using (var server = CreateServer())
            {
                
                IAppDbContext context = server.Host.Services.GetService(typeof(IAppDbContext)) as IAppDbContext;
                Behaviour behaviour = default(Behaviour);
                Frequency frequency = default(Frequency);

                context.Behaviours.Add(behaviour = new Behaviour()
                {
                    Name = "Running",
                    BehaviourTypeId = context.BehaviourTypes.Single(x => x.Name == "Health").BehaviourTypeId
                });

                var frequencyType = context.FrequencyTypes.Single(x => x.Name == "per day");

                context.Frequencies.Add(frequency = new Frequency()
                {
                    FrequencyType = frequencyType,
                    IsDesirable = true,
                    Frequency = 1
                });

                context.Commitments.Add(new Commitment()
                {
                    Behaviour = behaviour,
                    ProfileId = 1,
                    CommitmentFrequencies = new List<CommitmentFrequency>() {
                        new CommitmentFrequency() { Frequency = frequency }
                    }                    
                });
                
                context.Activities.Add(new Activity()
                {
                    Behaviour = behaviour,
                    PerformedOn = DateTime.Now.Date,
                    ProfileId = 1
                });

                await context.SaveChangesAsync(default(CancellationToken));

                var response = await server.CreateClient()
                    .GetAsync<GetAchievementsQuery.Response>(Get.Achievements);

                Assert.Single(response.Achievements);
            }
        }

    }
}
