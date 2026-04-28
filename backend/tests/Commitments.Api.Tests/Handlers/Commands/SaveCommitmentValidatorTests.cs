using Commitments.Features.Commitment;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Commands;

public class SaveCommitmentValidatorTests
{
    private readonly SaveCommitmentCommandValidator _validator = new();

    [Fact]
    public void Validate_FailsWhenAnyPreConditionNameIsEmpty(/* design 09 Slice C / L2-009 AC#2 */)
    {
        var request = new SaveCommitmentRequest
        {
            Commitment = new CommitmentDto
            {
                CommitmentId = Guid.NewGuid(),
                PreConditions = new List<CommitmentPreConditionDto>
                {
                    new CommitmentPreConditionDto { Name = "ok" },
                    new CommitmentPreConditionDto { Name = "" }
                }
            }
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor("Commitment.PreConditions[1].Name");
    }

    [Fact]
    public void Validate_PassesWhenAllPreConditionNamesAreSet()
    {
        var request = new SaveCommitmentRequest
        {
            Commitment = new CommitmentDto
            {
                CommitmentId = Guid.NewGuid(),
                PreConditions = new List<CommitmentPreConditionDto>
                {
                    new CommitmentPreConditionDto { Name = "Wake up" },
                    new CommitmentPreConditionDto { Name = "Make bed" }
                }
            }
        };

        var result = _validator.TestValidate(request);

        result.IsValid.Should().BeTrue();
    }
}
