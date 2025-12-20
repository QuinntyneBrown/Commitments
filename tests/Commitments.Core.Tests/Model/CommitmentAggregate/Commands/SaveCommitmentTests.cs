// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Model.CommitmentAggregate;
using Commitments.Core.Model.CommitmentAggregate.Commands;
using Commitments.Core.Tests.Helpers;
using FluentAssertions;
using Moq;
using Xunit;

namespace Commitments.Core.Tests.AggregateModel.CommitmentAggregate.Commands;

public class SaveCommitmentTests
{
    [Fact]
    public void SaveCommitmentCommandValidator_ShouldPassWhenCommitmentIdIsSet()
    {
        // Arrange
        var validator = new SaveCommitmentCommandValidator();
        var request = new SaveCommitmentRequest
        {
            Commitment = new CommitmentDto
            {
                CommitmentId = Guid.NewGuid()
            }
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void SaveCommitmentCommandValidator_ShouldFailWhenCommitmentIdIsNotSet()
    {
        // Arrange
        var validator = new SaveCommitmentCommandValidator();
        var request = new SaveCommitmentRequest
        {
            Commitment = new CommitmentDto
            {
                CommitmentId = default
            }
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task SaveCommitmentCommandHandler_ShouldCreateNewCommitment()
    {
        // Arrange
        var mockContext = MockDbContextFactory.Create();
        var commitments = new List<Commitment>();
        var mockDbSet = MockDbContextFactory.CreateMockDbSet(commitments);
        mockContext.Setup(c => c.Commitments).Returns(mockDbSet.Object);

        var handler = new SaveCommitmentCommandHandler(mockContext.Object);
        var request = new SaveCommitmentRequest
        {
            Commitment = new CommitmentDto
            {
                CommitmentId = Guid.NewGuid(),
                BehaviourId = Guid.NewGuid(),
                ProfileId = Guid.NewGuid()
            }
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void SaveCommitmentRequest_ShouldSetCommitment()
    {
        // Arrange
        var request = new SaveCommitmentRequest();
        var commitment = new CommitmentDto { CommitmentId = Guid.NewGuid() };

        // Act
        request.Commitment = commitment;

        // Assert
        request.Commitment.Should().Be(commitment);
    }

    [Fact]
    public void SaveCommitmentResponse_ShouldSetCommitmentId()
    {
        // Arrange
        var response = new SaveCommitmentResponse();
        var commitmentId = Guid.NewGuid();

        // Act
        response.CommitmentId = commitmentId;

        // Assert
        response.CommitmentId.Should().Be(commitmentId);
    }
}
