using Commitments.API.Features.DashboardCards;
using Commitments.Core.Entities;
using Commitments.Core.Extensions;
using Commitments.Core.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Features
{
    public class DashboardCardScenarios: DashboardCardScenarioBase
    {

        [Fact]
        public async Task ShouldSave()
        {
            using (var server = CreateServer())
            {
                IAppDbContext context = server.Host.Services.GetService(typeof(IAppDbContext)) as IAppDbContext;

                var response = await server.CreateClient()
                    .PostAsAsync<SaveDashboardCardCommand.Request, SaveDashboardCardCommand.Response>(Post.DashboardCards, new SaveDashboardCardCommand.Request() {
                        DashboardCard = new DashboardCardApiModel()
                        {
                            DashboardId = 1,
                            CardId = 1,
                            Options = new DashboardCardApiModel.OptionsApiModel()
                            {
                                Top = 1,
                                Left = 1,
                                Width = 1,
                                Height = 1
                            }
                        }
                    });
     
	            var entity = context.DashboardCards.First();

                Assert.Equal(1, entity.DashboardId);
            }
        }

        [Fact]
        public async Task ShouldGetAll()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetDashboardCardsQuery.Response>(Get.DashboardCards);

                Assert.True(response.DashboardCards.Count() > 0);
            }
        }


        [Fact]
        public async Task ShouldGetById()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetDashboardCardByIdQuery.Response>(Get.DashboardCardById(1));

                Assert.True(response.DashboardCard.DashboardCardId != default(int));
            }
        }
        
        [Fact]
        public async Task ShouldUpdate()
        {
            using (var server = CreateServer())
            {
                var getByIdResponse = await server.CreateClient()
                    .GetAsync<GetDashboardCardByIdQuery.Response>(Get.DashboardCardById(1));

                Assert.True(getByIdResponse.DashboardCard.DashboardCardId != default(int));

                var saveResponse = await server.CreateClient()
                    .PostAsAsync<SaveDashboardCardCommand.Request, SaveDashboardCardCommand.Response>(Post.DashboardCards, new SaveDashboardCardCommand.Request()
                    {
                        DashboardCard = getByIdResponse.DashboardCard
                    });

                Assert.True(saveResponse.DashboardCardId != default(int));
            }
        }
        
        [Fact]
        public async Task ShouldDelete()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .DeleteAsync(Delete.DashboardCard(1));

                response.EnsureSuccessStatusCode();
            }
        }
    }
}
