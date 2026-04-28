using Commitments.Data;
using Commitments.Domain.ActivityAggregate;
using Commitments.Domain.BehaviourAggregate;
using Commitments.Domain.BehaviourTypeAggregate;
using Commitments.Domain.CommitmentAggregate;
using Commitments.Domain.FrequencyAggregate;
using Commitments.Domain.FrequencyTypeAggregate;
using Commitments.Domain.ToDoAggregate;
using Dashboard.Data;
using Dashboard.Domain.DashboardAggregate;
using Dashboard.Domain.DashboardCardAggregate;
using Dashboard.Domain.CardAggregate;
using Dashboard.Domain.CardLayoutAggregate;
using Identity.Data;
using Identity.Domain.UserAggregate;
using Identity.Domain.ProfileAggregate;
using DigitalAssets.Data;
using DigitalAssets.Domain.DigitalAssetAggregate;
using Moq;

namespace Commitments.Testing.Common;

public static class MockCommitmentsDbContextFactory
{
    public static Mock<ICommitmentsDbContext> Create()
    {
        var mockContext = new Mock<ICommitmentsDbContext>();
        mockContext.Setup(c => c.Activities).Returns(MockDbSetFactory.CreateMockDbSet<Activity>().Object);
        mockContext.Setup(c => c.Behaviours).Returns(MockDbSetFactory.CreateMockDbSet<Behaviour>().Object);
        mockContext.Setup(c => c.BehaviourTypes).Returns(MockDbSetFactory.CreateMockDbSet<BehaviourType>().Object);
        mockContext.Setup(c => c.Commitments).Returns(MockDbSetFactory.CreateMockDbSet<Commitment>().Object);
        mockContext.Setup(c => c.CommitmentFrequencies).Returns(MockDbSetFactory.CreateMockDbSet<CommitmentFrequency>().Object);
        mockContext.Setup(c => c.Frequencies).Returns(MockDbSetFactory.CreateMockDbSet<Frequency>().Object);
        mockContext.Setup(c => c.FrequencyTypes).Returns(MockDbSetFactory.CreateMockDbSet<FrequencyType>().Object);
        mockContext.Setup(c => c.Todos).Returns(MockDbSetFactory.CreateMockDbSet<ToDo>().Object);
        mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        return mockContext;
    }
}

public static class MockIdentityDbContextFactory
{
    public static Mock<IIdentityDbContext> Create()
        => Create(new List<User>(), new List<Profile>());

    public static Mock<IIdentityDbContext> Create(List<User> users)
        => Create(users, new List<Profile>());

    public static Mock<IIdentityDbContext> Create(List<User> users, List<Profile> profiles)
    {
        var mockContext = new Mock<IIdentityDbContext>();
        mockContext.Setup(c => c.Users).Returns(MockDbSetFactory.CreateMockDbSet(users).Object);
        mockContext.Setup(c => c.Profiles).Returns(MockDbSetFactory.CreateMockDbSet(profiles).Object);
        mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        return mockContext;
    }
}

public static class MockDashboardDbContextFactory
{
    public static Mock<IDashboardDbContext> Create()
    {
        var mockContext = new Mock<IDashboardDbContext>();
        mockContext.Setup(c => c.Dashboards).Returns(MockDbSetFactory.CreateMockDbSet<DashboardEntity>().Object);
        mockContext.Setup(c => c.DashboardCards).Returns(MockDbSetFactory.CreateMockDbSet<DashboardCard>().Object);
        mockContext.Setup(c => c.Cards).Returns(MockDbSetFactory.CreateMockDbSet<Card>().Object);
        mockContext.Setup(c => c.CardLayouts).Returns(MockDbSetFactory.CreateMockDbSet<CardLayout>().Object);
        mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        return mockContext;
    }
}

public static class MockDigitalAssetsDbContextFactory
{
    public static Mock<IDigitalAssetsDbContext> Create()
    {
        var mockContext = new Mock<IDigitalAssetsDbContext>();
        mockContext.Setup(c => c.DigitalAssets).Returns(MockDbSetFactory.CreateMockDbSet<DigitalAsset>().Object);
        mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        return mockContext;
    }
}
