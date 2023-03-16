using Commitments.Api.Features.BehaviourTypes;
using Commitments.Core.Entities;
using Commitments.Core.Extensions;
using Commitments.Core.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;


namespace IntegrationTests.Features;

public class BehaviourTypeScenarios: BehaviourTypeScenarioBase
{

    [Fact]
    public async Task ShouldSave()
    {
        using (var server = CreateServer())
        {
            IAppDbContext context = server.Host.Services.GetService(typeof(IAppDbContext)) as IAppDbContext;

            var response = await server.CreateClient()
                .PostAsAsync<SaveBehaviourTypeCommand.Request, SaveBehaviourTypeCommand.Response>(Post.BehaviourTypes, new SaveBehaviourTypeCommand.Request() {
                    BehaviourType = new BehaviourTypeApiModel()
                    {
                        Name = "Name",
                    }
                });

            var entity = context.BehaviourTypes.First();

            Assert.Equal("Name", entity.Name);
        }
    }

    [Fact]
    public async Task ShouldGetAll()
    {
        using (var server = CreateServer())
        {
            var response = await server.CreateClient()
                .GetAsync<GetBehaviourTypesQuery.Response>(Get.BehaviourTypes);

            Assert.True(response.BehaviourTypes.Count() > 0);
        }
    }


    [Fact]
    public async Task ShouldGetById()
    {
        using (var server = CreateServer())
        {
            var response = await server.CreateClient()
                .GetAsync<GetBehaviourTypeByIdQuery.Response>(Get.BehaviourTypeById(1));

            Assert.True(response.BehaviourType.BehaviourTypeId != default(int));
        }
    }

    [Fact]
    public async Task ShouldUpdate()
    {
        using (var server = CreateServer())
        {
            var getByIdResponse = await server.CreateClient()
                .GetAsync<GetBehaviourTypeByIdQuery.Response>(Get.BehaviourTypeById(1));

            Assert.True(getByIdResponse.BehaviourType.BehaviourTypeId != default(int));

            var saveResponse = await server.CreateClient()
                .PostAsAsync<SaveBehaviourTypeCommand.Request, SaveBehaviourTypeCommand.Response>(Post.BehaviourTypes, new SaveBehaviourTypeCommand.Request()
                {
                    BehaviourType = getByIdResponse.BehaviourType
                });

            Assert.True(saveResponse.BehaviourTypeId != default(int));
        }
    }

    [Fact]
    public async Task ShouldDelete()
    {
        using (var server = CreateServer())
        {
            var response = await server.CreateClient()
                .DeleteAsync(Delete.BehaviourType(1));

            response.EnsureSuccessStatusCode();
        }
    }
}
