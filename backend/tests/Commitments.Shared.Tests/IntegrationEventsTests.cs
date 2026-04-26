using Commitments.Shared;
using FluentAssertions;
using Xunit;

namespace Commitments.Shared.Tests;

public class IntegrationEventsTests
{
    [Fact]
    public void ProfileCreatedEvent_ShouldSetProperties()
    {
        var id = Guid.NewGuid();
        var @event = new ProfileCreatedEvent { ProfileId = id };

        @event.ProfileId.Should().Be(id);
        @event.Id.Should().NotBeEmpty();
        @event.OccurredOn.Should().NotBe(default);
    }

    [Fact]
    public void ProfileDeletedEvent_ShouldSetProperties()
    {
        var id = Guid.NewGuid();
        var @event = new ProfileDeletedEvent { ProfileId = id };

        @event.ProfileId.Should().Be(id);
        @event.Id.Should().NotBeEmpty();
        @event.OccurredOn.Should().NotBe(default);
    }

    [Fact]
    public void UserCreatedEvent_ShouldSetProperties()
    {
        var id = Guid.NewGuid();
        var @event = new UserCreatedEvent { UserId = id, Username = "testuser" };

        @event.UserId.Should().Be(id);
        @event.Username.Should().Be("testuser");
        @event.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void IntegrationEvent_ShouldImplementIIntegrationEvent()
    {
        var @event = new ProfileCreatedEvent();

        @event.Should().BeAssignableTo<IIntegrationEvent>();
    }

    [Fact]
    public void ResponseBase_ShouldHaveEmptyErrorsByDefault()
    {
        var response = new ResponseBase();

        response.Errors.Should().NotBeNull();
        response.Errors.Should().BeEmpty();
    }

    [Fact]
    public void ResponseBase_ShouldAccumulateErrors()
    {
        var response = new ResponseBase();
        response.Errors.Add("Error 1");
        response.Errors.Add("Error 2");

        response.Errors.Should().HaveCount(2);
    }
}
