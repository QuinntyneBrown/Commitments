// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Model.BehaviourAggregate;
using Commitments.Core.Model.BehaviourTypeAggregate;
using Commitments.Core.Model.CommitmentAggregate;
using Commitments.Api.Features.Commitment;
using Commitments.Core.Tests.Helpers;
using FluentAssertions;
using Moq;
using Xunit;

namespace Commitments.Core.Tests.AggregateModel.CommitmentAggregate.Queries;

public class GetCommitmentsTests
{
    [Fact]
    public async Task GetCommitmentsQueryHandler_ShouldReturnEmptyListWhenNoCommitments()
    {
        // Arrange
        var mockContext = MockDbContextFactory.Create();
        var commitments = new List<Commitment>();
        var mockDbSet = MockDbContextFactory.CreateMockDbSet(commitments);
        mockContext.Setup(c => c.Commitments).Returns(mockDbSet.Object);

        var handler = new GetCommitmentsQueryHandler(mockContext.Object);
        var request = new GetCommitmentsRequest();

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Commitments.Should().BeEmpty();
    }

    [Fact]
    public async Task GetCommitmentsQueryHandler_ShouldReturnCommitments()
    {
        // Arrange
        var mockContext = MockDbContextFactory.Create();

        var behaviourType = new BehaviourType
        {
            BehaviourTypeId = Guid.NewGuid(),
            Name = "Test Type"
        };

        var behaviour = new Behaviour
        {
            BehaviourId = Guid.NewGuid(),
            Name = "Test Behaviour",
            BehaviourType = behaviourType
        };

        var commitments = new List<Commitment>
        {
            new Commitment
            {
                CommitmentId = Guid.NewGuid(),
                BehaviourId = behaviour.BehaviourId,
                ProfileId = Guid.NewGuid(),
                Behaviour = behaviour
            }
        };

        var mockDbSet = MockDbContextFactory.CreateMockDbSet(commitments);
        mockContext.Setup(c => c.Commitments).Returns(mockDbSet.Object);

        var handler = new GetCommitmentsQueryHandler(mockContext.Object);
        var request = new GetCommitmentsRequest();

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Commitments.Should().HaveCount(1);
    }

    [Fact]
    public void GetCommitmentsRequest_ShouldBeCreatable()
    {
        // Arrange & Act
        var request = new GetCommitmentsRequest();

        // Assert
        request.Should().NotBeNull();
    }

    [Fact]
    public void GetCommitmentsResponse_ShouldSetCommitments()
    {
        // Arrange
        var response = new GetCommitmentsResponse();
        var commitments = new List<CommitmentDto>
        {
            new CommitmentDto { CommitmentId = Guid.NewGuid() }
        };

        // Act
        response.Commitments = commitments;

        // Assert
        response.Commitments.Should().HaveCount(1);
    }
}
