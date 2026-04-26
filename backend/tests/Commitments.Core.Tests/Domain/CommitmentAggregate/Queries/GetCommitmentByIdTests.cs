// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Domain.CommitmentAggregate;
using Commitments.Features.Commitment;
using FluentAssertions;
using Xunit;

namespace Commitments.Domain.Tests.AggregateModel.CommitmentAggregate.Queries;

public class GetCommitmentByIdTests
{
    [Fact]
    public void GetCommitmentByIdValidator_ShouldPassWhenCommitmentIdIsValid()
    {
        // Arrange
        var validator = new GetCommitmentByIdValidator();
        var request = new GetCommitmentByIdRequest
        {
            CommitmentId = Guid.NewGuid()
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void GetCommitmentByIdValidator_ShouldFailWhenCommitmentIdIsDefault()
    {
        // Arrange
        var validator = new GetCommitmentByIdValidator();
        var request = new GetCommitmentByIdRequest
        {
            CommitmentId = default
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void GetCommitmentByIdRequest_ShouldSetCommitmentId()
    {
        // Arrange
        var request = new GetCommitmentByIdRequest();
        var commitmentId = Guid.NewGuid();

        // Act
        request.CommitmentId = commitmentId;

        // Assert
        request.CommitmentId.Should().Be(commitmentId);
    }

    [Fact]
    public void GetCommitmentByIdResponse_ShouldSetCommitment()
    {
        // Arrange
        var response = new GetCommitmentByIdResponse();
        var commitment = new CommitmentDto { CommitmentId = Guid.NewGuid() };

        // Act
        response.Commitment = commitment;

        // Assert
        response.Commitment.Should().Be(commitment);
    }
}
