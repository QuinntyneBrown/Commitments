// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Model.CommitmentAggregate.Commands;
using FluentAssertions;
using Xunit;

namespace Commitments.Core.Tests.AggregateModel.CommitmentAggregate.Commands;

public class RemoveCommitmentTests
{
    [Fact]
    public void RemoveCommitmentCommandValidator_ShouldPassWhenCommitmentIdIsValid()
    {
        // Arrange
        var validator = new RemoveCommitmentCommandValidator();
        var request = new RemoveCommitmentRequest
        {
            CommitmentId = Guid.NewGuid()
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void RemoveCommitmentCommandValidator_ShouldFailWhenCommitmentIdIsDefault()
    {
        // Arrange
        var validator = new RemoveCommitmentCommandValidator();
        var request = new RemoveCommitmentRequest
        {
            CommitmentId = default
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void RemoveCommitmentRequest_ShouldSetCommitmentId()
    {
        // Arrange
        var request = new RemoveCommitmentRequest();
        var commitmentId = Guid.NewGuid();

        // Act
        request.CommitmentId = commitmentId;

        // Assert
        request.CommitmentId.Should().Be(commitmentId);
    }
}
