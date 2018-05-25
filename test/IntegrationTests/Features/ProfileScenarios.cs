using Commitments.API.Features.Profiles;
using Commitments.Core.Entities;
using Commitments.Core.Extensions;
using Commitments.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Features
{
    public class ProfileScenarios: ProfileScenarioBase
    {

        [Fact]
        public async Task ShouldCreate()
        {
            using (var server = CreateServer())
            {
                IAppDbContext context = server.Host.Services.GetService(typeof(IAppDbContext)) as IAppDbContext;

                var response = await server.CreateClient()
                    .PostAsAsync<CreateProfileCommand.Request, CreateProfileCommand.Response>(Post.Create, new CreateProfileCommand.Request() {

                        Username ="quinntyne@hotmail.com",
                        Password = "P@ssword",
                        ConfirmPassword = "P@ssword"
                    });
     
	            var entity = context.Profiles
                    .Include(x => x.User)
                    .Single(x =>x.Name == "quinntyne@hotmail.com");

                Assert.Equal("quinntyne@hotmail.com", entity.Name);
            }
        }

    }
}
