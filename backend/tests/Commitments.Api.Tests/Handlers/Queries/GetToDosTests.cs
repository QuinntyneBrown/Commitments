using Commitments.Data;
using Commitments.Domain.ToDoAggregate;
using Commitments.Features.ToDo;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Queries;

public class GetToDosTests
{
    [Fact]
    public async Task Handle_ReturnsCallerToDosOrderedOutstandingFirstThenDueOnAsc(/* design 11 Slice B */)
    {
        var caller = Guid.NewGuid();
        var other = Guid.NewGuid();

        var options = new DbContextOptionsBuilder<CommitmentsDbContext>()
            .UseInMemoryDatabase($"get-todos-{Guid.NewGuid()}").Options;
        await using var ctx = new CommitmentsDbContext(options);

        var completed = Guid.NewGuid();
        var earlyDue = Guid.NewGuid();
        var lateDue = Guid.NewGuid();
        var otherProfile = Guid.NewGuid();

        ctx.Todos.Add(new ToDo { ToDoId = completed, ProfileId = caller, Title = "done", CompletedOn = DateTime.UtcNow });
        ctx.Todos.Add(new ToDo { ToDoId = lateDue,   ProfileId = caller, Title = "later", DueOn = new DateTime(2026, 12, 31) });
        ctx.Todos.Add(new ToDo { ToDoId = earlyDue,  ProfileId = caller, Title = "soon",  DueOn = new DateTime(2026, 6, 1) });
        ctx.Todos.Add(new ToDo { ToDoId = otherProfile, ProfileId = other, Title = "nope" });
        await ctx.SaveChangesAsync(CancellationToken.None);

        var handler = new GetToDosQueryHandler(ctx);

        var response = await handler.Handle(
            new GetToDosRequest { ProfileId = caller },
            CancellationToken.None);

        response.ToDos.Select(t => t.ToDoId).Should().Equal(earlyDue, lateDue, completed);
    }
}
