using Commitments.API.Features.CommitmentFrequencies;
using Commitments.Core.Entities;
using Commitments.Core.Extensions;
using Commitments.Core.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Features
{
    public class CommitmentFrequencyScenarios: CommitmentFrequencyScenarioBase
    {

        [Fact]
        public async Task ShouldSave()
        {
            using (var server = CreateServer())
            {
                IAppDbContext context = server.Host.Services.GetService(typeof(IAppDbContext)) as IAppDbContext;

                context.Behaviours.Add(new Behaviour()
                {
                    Name = "Running"
                });

                context.Commitments.Add(new Commitment()
                {
                    ProfileId = 1,
                    BehaviourId = 1
                });

                await context.SaveChangesAsync(default(CancellationToken));

                var response = await server.CreateClient()
                    .PostAsAsync<SaveCommitmentFrequencyCommand.Request, SaveCommitmentFrequencyCommand.Response>(Post.CommitmentFrequencies, new SaveCommitmentFrequencyCommand.Request() {
                        CommitmentFrequency = new CommitmentFrequencyApiModel()
                        {
                            Frequency = 1,
                            FrequencyTypeId = 1,
                            IsDesirable = true,
                            CommitmentId  = 1
                        }
                    });
     
	            var entity = context.CommitmentFrequencies.First();

                Assert.Equal(1, entity.Frequency);
            }
        }

        [Fact]
        public async Task ShouldGetAll()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetCommitmentFrequenciesQuery.Response>(Get.CommitmentFrequencies);

                Assert.True(response.CommitmentFrequencies.Count() > 0);
            }
        }


        [Fact]
        public async Task ShouldGetById()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetCommitmentFrequencyByIdQuery.Response>(Get.CommitmentFrequencyById(1));

                Assert.True(response.CommitmentFrequency.CommitmentFrequencyId != default(int));
            }
        }
        
        [Fact]
        public async Task ShouldUpdate()
        {
            using (var server = CreateServer())
            {
                var getByIdResponse = await server.CreateClient()
                    .GetAsync<GetCommitmentFrequencyByIdQuery.Response>(Get.CommitmentFrequencyById(1));

                Assert.True(getByIdResponse.CommitmentFrequency.CommitmentFrequencyId != default(int));

                var saveResponse = await server.CreateClient()
                    .PostAsAsync<SaveCommitmentFrequencyCommand.Request, SaveCommitmentFrequencyCommand.Response>(Post.CommitmentFrequencies, new SaveCommitmentFrequencyCommand.Request()
                    {
                        CommitmentFrequency = getByIdResponse.CommitmentFrequency
                    });

                Assert.True(saveResponse.CommitmentFrequencyId != default(int));
            }
        }
        
        [Fact]
        public async Task ShouldDelete()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .DeleteAsync(Delete.CommitmentFrequency(1));

                response.EnsureSuccessStatusCode();
            }
        }
    }
}
