using Commitments.API.Features.Activities;
using Commitments.Core.Entities;
using Commitments.Core.Extensions;
using Commitments.Core.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Features
{
    public class ActivityScenarios: ActivityScenarioBase
    {

        [Fact]
        public async Task ShouldSave()
        {
            using (var server = CreateServer())
            {
                IAppDbContext context = server.Host.Services.GetService(typeof(IAppDbContext)) as IAppDbContext;

                context.Behaviours.Add(new Behaviour()
                {
                    Name = "Running",
                    BehaviourTypeId = context.BehaviourTypes.Single(x => x.Name == "Health").BehaviourTypeId
                });

                await context.SaveChangesAsync(default(CancellationToken));

                var response = await server.CreateClient()
                    .PostAsAsync<SaveActivityCommand.Request, SaveActivityCommand.Response>(Post.Activities, new SaveActivityCommand.Request() {
                        Activity = new ActivityApiModel()
                        {
                            ProfileId = 1,
                            BehaviourId = 1,
                            PerformedOn = DateTime.UtcNow
                        }
                    });
     
	            var entity = context.Activities.First();

                Assert.Equal(1, entity.ProfileId);
                Assert.Equal(1, entity.BehaviourId);
            }
        }

        [Fact]
        public async Task ShouldGetAll()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetActivitiesQuery.Response>(Get.Activities);

                Assert.True(response.Activities.Count() > 0);
            }
        }


        [Fact]
        public async Task ShouldGetById()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetActivityByIdQuery.Response>(Get.ActivityById(1));

                Assert.True(response.Activity.ActivityId != default(int));
            }
        }
        
        [Fact]
        public async Task ShouldUpdate()
        {
            using (var server = CreateServer())
            {
                var getByIdResponse = await server.CreateClient()
                    .GetAsync<GetActivityByIdQuery.Response>(Get.ActivityById(1));

                Assert.True(getByIdResponse.Activity.ActivityId != default(int));

                var saveResponse = await server.CreateClient()
                    .PostAsAsync<SaveActivityCommand.Request, SaveActivityCommand.Response>(Post.Activities, new SaveActivityCommand.Request()
                    {
                        Activity = getByIdResponse.Activity
                    });

                Assert.True(saveResponse.ActivityId != default(int));
            }
        }
        
        [Fact]
        public async Task ShouldDelete()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .DeleteAsync(Delete.Activity(1));

                response.EnsureSuccessStatusCode();
            }
        }
    }
}
