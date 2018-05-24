using Commitments.API.Features.ToDos;
using Commitments.Core.Entities;
using Commitments.Core.Extensions;
using Commitments.Core.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Features
{
    public class ToDoScenarios: ToDoScenarioBase
    {

        [Fact]
        public async Task ShouldSave()
        {
            using (var server = CreateServer())
            {
                IAppDbContext context = server.Host.Services.GetService(typeof(IAppDbContext)) as IAppDbContext;

                var response = await server.CreateClient()
                    .PostAsAsync<SaveToDoCommand.Request, SaveToDoCommand.Response>(Post.ToDos, new SaveToDoCommand.Request() {
                        ToDo = new ToDoApiModel()
                        {
                            Name = "Name",
                            ProfileId = 1
                        }
                    });
     
	            var entity = context.ToDos.First();

                Assert.Equal("Name", entity.Name);
            }
        }

        [Fact]
        public async Task ShouldGetAll()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetToDosQuery.Response>(Get.ToDos);

                Assert.True(response.ToDos.Count() > 0);
            }
        }


        [Fact]
        public async Task ShouldGetById()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetToDoByIdQuery.Response>(Get.ToDoById(1));

                Assert.True(response.ToDo.ToDoId != default(int));
            }
        }
        
        [Fact]
        public async Task ShouldUpdate()
        {
            using (var server = CreateServer())
            {
                var getByIdResponse = await server.CreateClient()
                    .GetAsync<GetToDoByIdQuery.Response>(Get.ToDoById(1));

                Assert.True(getByIdResponse.ToDo.ToDoId != default(int));

                var saveResponse = await server.CreateClient()
                    .PostAsAsync<SaveToDoCommand.Request, SaveToDoCommand.Response>(Post.ToDos, new SaveToDoCommand.Request()
                    {
                        ToDo = getByIdResponse.ToDo
                    });

                Assert.True(saveResponse.ToDoId != default(int));
            }
        }
        
        [Fact]
        public async Task ShouldDelete()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .DeleteAsync(Delete.ToDo(1));

                response.EnsureSuccessStatusCode();
            }
        }
    }
}
