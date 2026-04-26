using Microsoft.EntityFrameworkCore;
using Moq;

namespace Commitments.Testing.Common;

public static class MockDbSetFactory
{
    public static Mock<DbSet<T>> CreateMockDbSet<T>() where T : class
    {
        return CreateMockDbSet(new List<T>());
    }

    public static Mock<DbSet<T>> CreateMockDbSet<T>(List<T> data) where T : class
    {
        var queryable = data.AsQueryable();
        var mockSet = new Mock<DbSet<T>>();

        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<T>(queryable.Provider));
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
        mockSet.As<IAsyncEnumerable<T>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(new TestAsyncEnumerator<T>(queryable.GetEnumerator()));
        mockSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>(data.Add);

        return mockSet;
    }
}
