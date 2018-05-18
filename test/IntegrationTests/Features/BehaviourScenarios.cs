using Commitments.API.Features.Behaviours;
using Commitments.Core.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Features
{
    public class BehaviourScenarios: BehaviourScenarioBase
    {

        [Fact]
        public async Task ShouldSave()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .PostAsAsync<SaveBehaviourCommand.Request, SaveBehaviourCommand.Response>(Post.Behaviours, new SaveBehaviourCommand.Request() {
                        Behaviour = new BehaviourApiModel()
                        {

                        }
                    });

                Assert.True(response.BehaviourId != default(int));
            }
        }

        [Fact]
        public async Task ShouldGetAll()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetBehavioursQuery.Response>(Get.Behaviours);

                Assert.True(response.Behaviours.Count() > 0);
            }
        }


        [Fact]
        public async Task ShouldGetById()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetBehaviourByIdQuery.Response>(Get.BehaviourById(1));

                Assert.True(response.Behaviour.BehaviourId != default(int));
            }
        }
        
        [Fact]
        public async Task ShouldUpdate()
        {
            using (var server = CreateServer())
            {
                var getByIdResponse = await server.CreateClient()
                    .GetAsync<GetBehaviourByIdQuery.Response>(Get.BehaviourById(1));

                Assert.True(getByIdResponse.Behaviour.BehaviourId != default(int));

                var saveResponse = await server.CreateClient()
                    .PostAsAsync<SaveBehaviourCommand.Request, SaveBehaviourCommand.Response>(Post.Behaviours, new SaveBehaviourCommand.Request()
                    {
                        Behaviour = getByIdResponse.Behaviour
                    });

                Assert.True(saveResponse.BehaviourId != default(int));
            }
        }
        
        [Fact]
        public async Task ShouldDelete()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .DeleteAsync(Delete.Behaviour(1));

                response.EnsureSuccessStatusCode();
            }
        }
    }
}
