using Commitments.API.Features.Commitments;
using Commitments.Core.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Features
{
    public class CommitmentScenarios: CommitmentScenarioBase
    {

        [Fact]
        public async Task ShouldSave()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .PostAsAsync<SaveCommitmentCommand.Request, SaveCommitmentCommand.Response>(Post.Commitments, new SaveCommitmentCommand.Request() {
                        Commitment = new CommitmentApiModel()
                        {

                        }
                    });

                Assert.True(response.CommitmentId != default(int));
            }
        }

        [Fact]
        public async Task ShouldGetAll()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetCommitmentsQuery.Response>(Get.Commitments);

                Assert.True(response.Commitments.Count() > 0);
            }
        }


        [Fact]
        public async Task ShouldGetById()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetCommitmentByIdQuery.Response>(Get.CommitmentById(1));

                Assert.True(response.Commitment.CommitmentId != default(int));
            }
        }
        
        [Fact]
        public async Task ShouldUpdate()
        {
            using (var server = CreateServer())
            {
                var getByIdResponse = await server.CreateClient()
                    .GetAsync<GetCommitmentByIdQuery.Response>(Get.CommitmentById(1));

                Assert.True(getByIdResponse.Commitment.CommitmentId != default(int));

                var saveResponse = await server.CreateClient()
                    .PostAsAsync<SaveCommitmentCommand.Request, SaveCommitmentCommand.Response>(Post.Commitments, new SaveCommitmentCommand.Request()
                    {
                        Commitment = getByIdResponse.Commitment
                    });

                Assert.True(saveResponse.CommitmentId != default(int));
            }
        }
        
        [Fact]
        public async Task ShouldDelete()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .DeleteAsync(Delete.Commitment(1));

                response.EnsureSuccessStatusCode();
            }
        }
    }
}
