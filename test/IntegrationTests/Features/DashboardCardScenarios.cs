// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Api.Features.DashboardCards;
using Commitments.Core.Entities;
using Commitments.Core.Extensions;
using Commitments.Core.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;


namespace IntegrationTests.Features;

public class DashboardCardScenarios: DashboardCardScenarioBase
{

    [Fact]
    public async Task ShouldSave()
    {
        using (var server = CreateServer())
        {
            IAppDbContext context = server.Host.Services.GetService(typeof(IAppDbContext)) as IAppDbContext;

            context.Dashboards.Add(new Dashboard()
            {
                Name = "Default",
                ProfileId = 1
            });

            await context.SaveChangesAsync(default(CancellationToken));

            var response = await server.CreateClient()
                .PostAsAsync<SaveDashboardCardCommand.Request, SaveDashboardCardCommand.Response>(Post.DashboardCards, new SaveDashboardCardCommand.Request()
                {
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
    public async Task ShouldSaveRange()
    {
        using (var server = CreateServer())
        {
            IAppDbContext context = server.Host.Services.GetService(typeof(IAppDbContext)) as IAppDbContext;

            context.Dashboards.Add(new Dashboard()
            {
                Name = "Default",
                ProfileId = 1
            });

            await context.SaveChangesAsync(default(CancellationToken));

            var response = await server.CreateClient()
                .PostAsAsync<SaveDashboardCardRangeCommand.Request, SaveDashboardCardRangeCommand.Response>(Post.DashboardCardsRange, new SaveDashboardCardRangeCommand.Request()
                {
                    DashboardCards = new List<DashboardCardApiModel>() {
                        new DashboardCardApiModel()
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
                    }
                });

            var entity = context.DashboardCards.First();

            Assert.Equal(1, entity.DashboardId);
        }
    }

    [Fact]
    public async Task ShouldGetRange()
    {
        using (var server = CreateServer())
        {

            IAppDbContext context = server.Host.Services.GetService(typeof(IAppDbContext)) as IAppDbContext;
            var dashboard = default(Dashboard);

            context.Dashboards.Add(dashboard = new Dashboard()
            {
                Name = "Default",
                ProfileId = 1
            });

            context.DashboardCards.Add(new DashboardCard()
            {
                CardId =2,
                Dashboard = dashboard,
                Options = JsonConvert.SerializeObject(new DashboardCardApiModel.OptionsApiModel()
                {
                    Top = 1,
                    Left = 1,
                    Width = 1,
                    Height = 1
                })
            });

            context.DashboardCards.Add(new DashboardCard()
            {
                CardId = 3,
                Dashboard = dashboard,
                Options = JsonConvert.SerializeObject(new DashboardCardApiModel.OptionsApiModel()
                {
                    Top = 1,
                    Left = 1,
                    Width = 1,
                    Height = 1
                })

            });

            context.DashboardCards.Add(new DashboardCard()
            {
                CardId = 1,
                Dashboard = dashboard,
                Options = JsonConvert.SerializeObject(new DashboardCardApiModel.OptionsApiModel()
                {
                    Top = 1,
                    Left = 1,
                    Width = 1,
                    Height = 1
                })
            });

            await context.SaveChangesAsync(default(CancellationToken));

            var response = await server.CreateClient()
                .GetAsync<GetDashboardCardByIdsQuery.Response>(Get.DashboardCardByIds(new List<int>() { 1, 2 }));

            Assert.True(response.DashboardCards.Count() == 2);
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

