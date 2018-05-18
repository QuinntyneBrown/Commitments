using Commitments.API.Features.Behaviours;
using Commitments.Core.Entities;
using Commitments.Core.Extensions;
using Commitments.Core.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Features
{
    public class BehaviourScenarios: BehaviourScenarioBase
    {

        [Fact]
        public async Task ShouldSave()
        {
            using (var server = CreateServer())
            {
                IAppDbContext context = server.Host.Services.GetService(typeof(IAppDbContext)) as IAppDbContext;
                
                var response = await server.CreateClient()
                    .PostAsAsync<SaveBehaviourCommand.Request, SaveBehaviourCommand.Response>(Post.Behaviours, new SaveBehaviourCommand.Request() {
                        Behaviour = new BehaviourApiModel()
                        {
                            Name = "Running",
                            Description = "Running",                       
                        }
                    });

                var entity = context.Behaviours.First();

                Assert.Equal("Running", entity.Name);
                Assert.Equal("Running", entity.Description);
                Assert.Equal("running", entity.Slug);
            }
        }

        [Fact]
        public async Task ShouldGetAll()
        {
            using (var server = CreateServer())
            {
                IAppDbContext context = server.Host.Services.GetService(typeof(IAppDbContext)) as IAppDbContext;

                context.Behaviours.Add(new Behaviour()
                {
                    Name = "Running",
                    Description = "Running",
                    Slug = "slug"
                });

                await context.SaveChangesAsync(default(CancellationToken));

                var response = await server.CreateClient()
                    .GetAsync<GetBehavioursQuery.Response>(Get.Behaviours);

                Assert.Single(response.Behaviours);
            }
        }


        [Fact]
        public async Task ShouldGetById()
        {
            using (var server = CreateServer())
            {
                IAppDbContext context = server.Host.Services.GetService(typeof(IAppDbContext)) as IAppDbContext;

                context.Behaviours.Add(new Behaviour()
                {
                    Name = "Running",
                    Description = "Running",
                    Slug = "slug"
                });

                await context.SaveChangesAsync(default(CancellationToken));

                var response = await server.CreateClient()
                    .GetAsync<GetBehaviourByIdQuery.Response>(Get.BehaviourById(1));

                Assert.True(response.Behaviour.BehaviourId != default(int));
            }
        }
        
        [Fact]
        public async Task ShouldUpdate()
        {
            using (var server = CreateServer())
            {
                IAppDbContext context = server.Host.Services.GetService(typeof(IAppDbContext)) as IAppDbContext;

                context.Behaviours.Add(new Behaviour()
                {
                    Name = "Running",
                    Description = "Running",
                    Slug = "slug"
                });

                await context.SaveChangesAsync(default(CancellationToken));

                var response = await server.CreateClient()
                    .PostAsAsync<SaveBehaviourCommand.Request, SaveBehaviourCommand.Response>(Post.Behaviours, new SaveBehaviourCommand.Request()
                    {
                        Behaviour = new BehaviourApiModel()
                        {
                            BehaviourId = 1,
                            Name = "Jogging",
                            Description = "Joggin"
                        }
                    });

                Assert.Equal(1, response.BehaviourId);
            }
        }
        
        [Fact]
        public async Task ShouldDelete()
        {
            using (var server = CreateServer())
            {
                IAppDbContext context = server.Host.Services.GetService(typeof(IAppDbContext)) as IAppDbContext;

                context.Behaviours.Add(new Behaviour()
                {
                    Name = "Running",
                    Description = "Running",
                    Slug = "slug"
                });

                await context.SaveChangesAsync(default(CancellationToken));

                var response = await server.CreateClient()
                    .DeleteAsync(Delete.Behaviour(1));

                response.EnsureSuccessStatusCode();
            }
        }
    }
}
