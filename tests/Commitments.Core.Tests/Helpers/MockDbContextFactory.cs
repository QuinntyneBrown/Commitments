// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core;
using Commitments.Core.AggregateModel;
using Commitments.Core.AggregateModel.ActivityAggregate;
using Commitments.Core.AggregateModel.BehaviourAggregate;
using Commitments.Core.AggregateModel.BehaviourTypeAggregate;
using Commitments.Core.AggregateModel.CardAggregate;
using Commitments.Core.AggregateModel.CardLayoutAggregate;
using Commitments.Core.AggregateModel.CommitmentAggregate;
using Commitments.Core.AggregateModel.DashboardAggregate;
using Commitments.Core.AggregateModel.DashboardCardAggregate;
using Commitments.Core.AggregateModel.DigitalAssetAggregate;
using Commitments.Core.AggregateModel.FrequencyAggregate;
using Commitments.Core.AggregateModel.FrequencyTypeAggregate;
using Commitments.Core.AggregateModel.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Commitments.Core.Tests.Helpers;

public static class MockDbContextFactory
{
    public static Mock<ICommitmentsDbContext> Create()
    {
        var mockContext = new Mock<ICommitmentsDbContext>();

        // Setup empty DbSets
        mockContext.Setup(c => c.Activities).Returns(CreateMockDbSet<Activity>().Object);
        mockContext.Setup(c => c.Behaviours).Returns(CreateMockDbSet<Behaviour>().Object);
        mockContext.Setup(c => c.BehaviourTypes).Returns(CreateMockDbSet<BehaviourType>().Object);
        mockContext.Setup(c => c.Commitments).Returns(CreateMockDbSet<Commitment>().Object);
        mockContext.Setup(c => c.CommitmentFrequencies).Returns(CreateMockDbSet<CommitmentFrequency>().Object);
        mockContext.Setup(c => c.Frequencies).Returns(CreateMockDbSet<Frequency>().Object);
        mockContext.Setup(c => c.FrequencyTypes).Returns(CreateMockDbSet<FrequencyType>().Object);
        mockContext.Setup(c => c.Cards).Returns(CreateMockDbSet<Card>().Object);
        mockContext.Setup(c => c.CardLayouts).Returns(CreateMockDbSet<CardLayout>().Object);
        mockContext.Setup(c => c.Dashboards).Returns(CreateMockDbSet<Dashboard>().Object);
        mockContext.Setup(c => c.DashboardCards).Returns(CreateMockDbSet<DashboardCard>().Object);
        mockContext.Setup(c => c.Profiles).Returns(CreateMockDbSet<Profile>().Object);
        mockContext.Setup(c => c.ProfileCommitments).Returns(CreateMockDbSet<Commitment>().Object);
        mockContext.Setup(c => c.Users).Returns(CreateMockDbSet<User>().Object);
        mockContext.Setup(c => c.DigitalAssets).Returns(CreateMockDbSet<DigitalAsset>().Object);
        mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        return mockContext;
    }

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

internal class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
{
    private readonly IQueryProvider _inner;

    internal TestAsyncQueryProvider(IQueryProvider inner)
    {
        _inner = inner;
    }

    public IQueryable CreateQuery(System.Linq.Expressions.Expression expression)
    {
        return new TestAsyncEnumerable<TEntity>(expression);
    }

    public IQueryable<TElement> CreateQuery<TElement>(System.Linq.Expressions.Expression expression)
    {
        return new TestAsyncEnumerable<TElement>(expression);
    }

    public object? Execute(System.Linq.Expressions.Expression expression)
    {
        return _inner.Execute(expression);
    }

    public TResult Execute<TResult>(System.Linq.Expressions.Expression expression)
    {
        return _inner.Execute<TResult>(expression);
    }

    public TResult ExecuteAsync<TResult>(System.Linq.Expressions.Expression expression, CancellationToken cancellationToken = default)
    {
        var expectedResultType = typeof(TResult).GetGenericArguments()[0];
        var executionResult = typeof(IQueryProvider)
            .GetMethod(
                name: nameof(IQueryProvider.Execute),
                genericParameterCount: 1,
                types: new[] { typeof(System.Linq.Expressions.Expression) })!
            .MakeGenericMethod(expectedResultType)
            .Invoke(this, new[] { expression });

        return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))!
            .MakeGenericMethod(expectedResultType)
            .Invoke(null, new[] { executionResult })!;
    }
}

internal class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
{
    public TestAsyncEnumerable(IEnumerable<T> enumerable)
        : base(enumerable)
    { }

    public TestAsyncEnumerable(System.Linq.Expressions.Expression expression)
        : base(expression)
    { }

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
    }

    IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);
}

internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _inner;

    public TestAsyncEnumerator(IEnumerator<T> inner)
    {
        _inner = inner;
    }

    public ValueTask DisposeAsync()
    {
        _inner.Dispose();
        return ValueTask.CompletedTask;
    }

    public ValueTask<bool> MoveNextAsync()
    {
        return ValueTask.FromResult(_inner.MoveNext());
    }

    public T Current => _inner.Current;
}
