using Commitments.Shared;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Commitments.Shared.Tests;

public class TestEntity : BaseEntity
{
    public Guid TestEntityId { get; set; }
    public string Name { get; set; } = "";
}

public class TestDbContext : BaseDbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }
    public DbSet<TestEntity> TestEntities { get; set; } = null!;
}

public class BaseDbContextTests : IDisposable
{
    private readonly TestDbContext _context;

    public BaseDbContextTests()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new TestDbContext(options);
    }

    public void Dispose() => _context.Dispose();

    [Fact]
    public async Task AddingEntity_SetsCreatedOn()
    {
        var entity = new TestEntity { TestEntityId = Guid.NewGuid(), Name = "Test" };

        _context.TestEntities.Add(entity);
        await _context.SaveChangesAsync();

        entity.CreatedOn.Should().NotBe(default);
    }

    [Fact]
    public async Task AddingEntity_SetsLastModifiedOn()
    {
        var entity = new TestEntity { TestEntityId = Guid.NewGuid(), Name = "Test" };

        _context.TestEntities.Add(entity);
        await _context.SaveChangesAsync();

        entity.LastModifiedOn.Should().NotBe(default);
    }

    [Fact]
    public async Task ModifyingEntity_UpdatesLastModifiedOn_PreservesCreatedOn()
    {
        var entity = new TestEntity { TestEntityId = Guid.NewGuid(), Name = "Test" };
        _context.TestEntities.Add(entity);
        await _context.SaveChangesAsync();

        var originalCreatedOn = entity.CreatedOn;
        var originalModifiedOn = entity.LastModifiedOn;
        await Task.Delay(10);

        entity.Name = "Updated";
        await _context.SaveChangesAsync();

        entity.CreatedOn.Should().Be(originalCreatedOn);
        entity.LastModifiedOn.Should().BeOnOrAfter(originalModifiedOn);
    }

    [Fact]
    public async Task DeletingEntity_SetsIsDeletedTrue_ChangesToModified()
    {
        var entity = new TestEntity { TestEntityId = Guid.NewGuid(), Name = "Test" };
        _context.TestEntities.Add(entity);
        await _context.SaveChangesAsync();

        _context.TestEntities.Remove(entity);
        await _context.SaveChangesAsync();

        var deleted = await _context.TestEntities.IgnoreQueryFilters()
            .FirstOrDefaultAsync(e => e.TestEntityId == entity.TestEntityId);
        deleted.Should().NotBeNull();
        deleted!.IsDeleted.Should().BeTrue();
    }
}
