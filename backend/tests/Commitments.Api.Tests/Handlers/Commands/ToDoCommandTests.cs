using Commitments.Data;
using Commitments.Domain.ToDoAggregate;
using Commitments.Features.ToDo;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Commands;

public class ToDoCommandTests
{
    private static CommitmentsDbContext NewCtx() =>
        new(new DbContextOptionsBuilder<CommitmentsDbContext>()
            .UseInMemoryDatabase($"todo-{Guid.NewGuid()}").Options);

    [Fact]
    public async Task Save_InsertsNewToDo(/* design 11 Slice B */)
    {
        await using var ctx = NewCtx();
        var profileId = Guid.NewGuid();
        var handler = new SaveToDoCommandHandler(ctx);

        var response = await handler.Handle(
            new SaveToDoRequest { ToDo = new ToDoDto { Title = "Buy milk", ProfileId = profileId } },
            CancellationToken.None);

        response.ToDoId.Should().NotBe(Guid.Empty);
        (await ctx.Todos.SingleAsync()).Title.Should().Be("Buy milk");
    }

    [Fact]
    public async Task Save_UpdatesExistingToDo()
    {
        await using var ctx = NewCtx();
        var id = Guid.NewGuid();
        ctx.Todos.Add(new ToDo { ToDoId = id, ProfileId = Guid.NewGuid(), Title = "old" });
        await ctx.SaveChangesAsync(CancellationToken.None);

        var handler = new SaveToDoCommandHandler(ctx);
        await handler.Handle(
            new SaveToDoRequest { ToDo = new ToDoDto { ToDoId = id, Title = "new" } },
            CancellationToken.None);

        (await ctx.Todos.SingleAsync()).Title.Should().Be("new");
    }

    [Fact]
    public async Task SaveValidator_FailsOnEmptyTitle()
    {
        var validator = new SaveToDoCommandValidator();
        var result = validator.Validate(new SaveToDoRequest { ToDo = new ToDoDto { Title = "" } });
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task Remove_SetsDeletedOn(/* design 11 Slice B */)
    {
        await using var ctx = NewCtx();
        var id = Guid.NewGuid();
        ctx.Todos.Add(new ToDo { ToDoId = id, ProfileId = Guid.NewGuid(), Title = "x" });
        await ctx.SaveChangesAsync(CancellationToken.None);

        var handler = new RemoveToDoCommandHandler(ctx);
        await handler.Handle(new RemoveToDoRequest { ToDoId = id }, CancellationToken.None);

        var raw = await ctx.Todos.IgnoreQueryFilters().SingleAsync();
        raw.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task Complete_SetsCompletedOn_AndIsIdempotent(/* design 11 Slice C */)
    {
        await using var ctx = NewCtx();
        var id = Guid.NewGuid();
        ctx.Todos.Add(new ToDo { ToDoId = id, ProfileId = Guid.NewGuid(), Title = "x" });
        await ctx.SaveChangesAsync(CancellationToken.None);

        var handler = new CompleteToDoCommandHandler(ctx);
        await handler.Handle(new CompleteToDoRequest { ToDoId = id }, CancellationToken.None);
        var firstCompletedOn = (await ctx.Todos.SingleAsync()).CompletedOn;
        firstCompletedOn.Should().NotBeNull();

        await Task.Delay(10);
        await handler.Handle(new CompleteToDoRequest { ToDoId = id }, CancellationToken.None);
        (await ctx.Todos.SingleAsync()).CompletedOn.Should().Be(firstCompletedOn);
    }
}
