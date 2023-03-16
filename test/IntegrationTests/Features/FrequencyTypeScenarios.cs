using Commitments.Api.Features.FrequencyTypes;
using Commitments.Core.Entities;
using Commitments.Core.Extensions;
using Commitments.Core.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;


namespace IntegrationTests.Features;

public class FrequencyTypeScenarios: FrequencyTypeScenarioBase
{

    [Fact]
    public async Task ShouldSave()
    {
        using (var server = CreateServer())
        {
            IAppDbContext context = server.Host.Services.GetService(typeof(IAppDbContext)) as IAppDbContext;

            var response = await server.CreateClient()
                .PostAsAsync<SaveFrequencyTypeCommand.Request, SaveFrequencyTypeCommand.Response>(Post.FrequencyTypes, new SaveFrequencyTypeCommand.Request() {
                    FrequencyType = new FrequencyTypeApiModel()
                    {
                        Name = "Name",
                    }
                });

            var entity = context.FrequencyTypes.First();

            Assert.Equal("Name", entity.Name);
        }
    }

    [Fact]
    public async Task ShouldGetAll()
    {
        using (var server = CreateServer())
        {
            var response = await server.CreateClient()
                .GetAsync<GetFrequencyTypesQuery.Response>(Get.FrequencyTypes);

            Assert.True(response.FrequencyTypes.Count() > 0);
        }
    }


    [Fact]
    public async Task ShouldGetById()
    {
        using (var server = CreateServer())
        {
            var response = await server.CreateClient()
                .GetAsync<GetFrequencyTypeByIdQuery.Response>(Get.FrequencyTypeById(1));

            Assert.True(response.FrequencyType.FrequencyTypeId != default(int));
        }
    }

    [Fact]
    public async Task ShouldUpdate()
    {
        using (var server = CreateServer())
        {
            var getByIdResponse = await server.CreateClient()
                .GetAsync<GetFrequencyTypeByIdQuery.Response>(Get.FrequencyTypeById(1));

            Assert.True(getByIdResponse.FrequencyType.FrequencyTypeId != default(int));

            var saveResponse = await server.CreateClient()
                .PostAsAsync<SaveFrequencyTypeCommand.Request, SaveFrequencyTypeCommand.Response>(Post.FrequencyTypes, new SaveFrequencyTypeCommand.Request()
                {
                    FrequencyType = getByIdResponse.FrequencyType
                });

            Assert.True(saveResponse.FrequencyTypeId != default(int));
        }
    }

    [Fact]
    public async Task ShouldDelete()
    {
        using (var server = CreateServer())
        {
            var response = await server.CreateClient()
                .DeleteAsync(Delete.FrequencyType(1));

            response.EnsureSuccessStatusCode();
        }
    }
}
