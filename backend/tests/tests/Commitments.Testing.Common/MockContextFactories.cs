using Commitments.Core;
using Commitments.Core.Model.ActivityAggregate;
using Commitments.Core.Model.BehaviourAggregate;
using Commitments.Core.Model.BehaviourTypeAggregate;
using Commitments.Core.Model.CommitmentAggregate;
using Commitments.Core.Model.FrequencyAggregate;
using Commitments.Core.Model.FrequencyTypeAggregate;
using Dashboard.Core;
using Dashboard.Core.Model.DashboardAggregate;
using Dashboard.Core.Model.DashboardCardAggregate;
using Dashboard.Core.Model.CardAggregate;
using Dashboard.Core.Model.CardLayoutAggregate;
using Identity.Core;
using Identity.Core.Model.UserAggregate;
using Identity.Core.Model.ProfileAggregate;
using DigitalAssets.Core;
using DigitalAssets.Core.Model.DigitalAssetAggregate;
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
        mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        return mockContext;
    }
}

public static class MockIdentityDbContextFactory
{
    public static Mock<IIdentityDbContext> Create()
    {
        var mockContext = new Mock<IIdentityDbContext>();
        mockContext.Setup(c => c.Users).Returns(MockDbSetFactory.CreateMockDbSet<User>().Object);
        mockContext.Setup(c => c.Profiles).Returns(MockDbSetFactory.CreateMockDbSet<Profile>().Object);
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
