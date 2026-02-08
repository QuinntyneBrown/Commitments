using Commitments.Shared;
using FluentAssertions;
using Xunit;

namespace Commitments.Shared.Tests;

public class BaseEntityTests
{
    private class TestEntity : BaseEntity { }

    [Fact]
    public void BaseEntity_ShouldHaveDefaultValues()
    {
        var entity = new TestEntity();

        entity.CreatedOn.Should().Be(default);
        entity.LastModifiedOn.Should().Be(default);
        entity.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public void BaseEntity_ShouldSetCreatedOn()
    {
        var entity = new TestEntity();
        var date = DateTime.UtcNow;

        entity.CreatedOn = date;

        entity.CreatedOn.Should().Be(date);
    }

    [Fact]
    public void BaseEntity_ShouldSetLastModifiedOn()
    {
        var entity = new TestEntity();
        var date = DateTime.UtcNow;

        entity.LastModifiedOn = date;

        entity.LastModifiedOn.Should().Be(date);
    }

    [Fact]
    public void BaseEntity_ShouldSetIsDeleted()
    {
        var entity = new TestEntity();

        entity.IsDeleted = true;

        entity.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public void BaseEntity_ShouldImplementILoggable()
    {
        var entity = new TestEntity();

        entity.Should().BeAssignableTo<ILoggable>();
    }
}
