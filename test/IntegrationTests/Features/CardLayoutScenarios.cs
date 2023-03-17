// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Api.Features.CardLayouts;
using Commitments.Core.Entities;
using Commitments.Core.Extensions;
using Commitments.Core.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;


namespace IntegrationTests.Features;

public class CardLayoutScenarios: CardLayoutScenarioBase
{

    [Fact]
    public async Task ShouldSave()
    {
        using (var server = CreateServer())
        {
            IAppDbContext context = server.Host.Services.GetService(typeof(IAppDbContext)) as IAppDbContext;

            var response = await server.CreateClient()
                .PostAsAsync<SaveCardLayoutCommand.Request, SaveCardLayoutCommand.Response>(Post.CardLayouts, new SaveCardLayoutCommand.Request() {
                    CardLayout = new CardLayoutApiModel()
                    {
                        Name = "Name",
                    }
                });


            Assert.Equal("Name", entity.Name);
        }
    }

    [Fact]
    public async Task ShouldGetAll()
    {
        using (var server = CreateServer())
        {
            var response = await server.CreateClient()
                .GetAsync<GetCardLayoutsQuery.Response>(Get.CardLayouts);

            Assert.True(response.CardLayouts.Count() > 0);
        }
    }


    [Fact]
    public async Task ShouldGetById()
    {
        using (var server = CreateServer())
        {
            var response = await server.CreateClient()
                .GetAsync<GetCardLayoutByIdQuery.Response>(Get.CardLayoutById(1));

            Assert.True(response.CardLayout.CardLayoutId != default(int));
        }
    }

    [Fact]
    public async Task ShouldUpdate()
    {
        using (var server = CreateServer())
        {
            var getByIdResponse = await server.CreateClient()
                .GetAsync<GetCardLayoutByIdQuery.Response>(Get.CardLayoutById(1));

            Assert.True(getByIdResponse.CardLayout.CardLayoutId != default(int));

            var saveResponse = await server.CreateClient()
                .PostAsAsync<SaveCardLayoutCommand.Request, SaveCardLayoutCommand.Response>(Post.CardLayouts, new SaveCardLayoutCommand.Request()
                {
                    CardLayout = getByIdResponse.CardLayout
                });

            Assert.True(saveResponse.CardLayoutId != default(int));
        }
    }

    [Fact]
    public async Task ShouldDelete()
    {
        using (var server = CreateServer())
        {
            var response = await server.CreateClient()
                .DeleteAsync(Delete.CardLayout(1));

            response.EnsureSuccessStatusCode();
        }
    }
}

