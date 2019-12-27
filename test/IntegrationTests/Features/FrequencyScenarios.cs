using Commitments.Api.Features.Frequencies;
using Commitments.Core.Entities;
using Commitments.Core.Extensions;
using Commitments.Core.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Features
{
    public class FrequencyScenarios: FrequencyScenarioBase
    {

        [Fact]
        public async Task ShouldSave()
        {
            using (var server = CreateServer())
            {
                IAppDbContext context = server.Host.Services.GetService(typeof(IAppDbContext)) as IAppDbContext;

                var response = await server.CreateClient()
                    .PostAsAsync<SaveFrequencyCommand.Request, SaveFrequencyCommand.Response>(Post.Frequencies, new SaveFrequencyCommand.Request() {
                        Frequency = new FrequencyApiModel()
                        {
                            Frequency = 0,
                            FrequencyTypeId = 1
                        }
                    });
     
                var entity = context.Frequencies.First();

                Assert.Equal(0, entity.Frequency);
            }
        }

        [Fact]
        public async Task ShouldGetAll()
        {
            using (var server = CreateServer())
            {
                IAppDbContext context = server.Host.Services.GetService(typeof(IAppDbContext)) as IAppDbContext;

                context.Frequencies.Add(new Frequency()
                {
                    Frequency = 1,
                    FrequencyTypeId = 1
                });

                await context.SaveChangesAsync(default(CancellationToken));

                var response = await server.CreateClient()
                    .GetAsync<GetFrequenciesQuery.Response>(Get.Frequencies);

                Assert.Single(response.Frequencies);
            }
        }


        [Fact]
        public async Task ShouldGetById()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetFrequencyByIdQuery.Response>(Get.FrequencyById(1));

                Assert.True(response.Frequency.FrequencyId != default(int));
            }
        }
        
        [Fact]
        public async Task ShouldUpdate()
        {
            using (var server = CreateServer())
            {
                var getByIdResponse = await server.CreateClient()
                    .GetAsync<GetFrequencyByIdQuery.Response>(Get.FrequencyById(1));

                Assert.True(getByIdResponse.Frequency.FrequencyId != default(int));

                var saveResponse = await server.CreateClient()
                    .PostAsAsync<SaveFrequencyCommand.Request, SaveFrequencyCommand.Response>(Post.Frequencies, new SaveFrequencyCommand.Request()
                    {
                        Frequency = getByIdResponse.Frequency
                    });

                Assert.True(saveResponse.FrequencyId != default(int));
            }
        }
        
        [Fact]
        public async Task ShouldDelete()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .DeleteAsync(Delete.Frequency(1));

                response.EnsureSuccessStatusCode();
            }
        }
    }
}
