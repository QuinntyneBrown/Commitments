using Commitments.API.Features.Profiles;
using Commitments.Core.Extensions;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Features
{
    public class ProfileScenarios: ProfileScenarioBase
    {        
        [Fact]
        public async Task ShouldGetCurrentProfile()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetProfileByUsernameQuery.Response>(Get.Current);

                Assert.True(response.Profile.ProfileId != default(int));
            }
        }        
    }
}
