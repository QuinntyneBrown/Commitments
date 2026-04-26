using Commitments.Features.Commitment;
using FluentAssertions;
using Xunit;

namespace Commitments.Api.Tests.Validators;

public class SaveCommitmentCommandValidatorTests
{
    private readonly SaveCommitmentCommandValidator _validator;

    public SaveCommitmentCommandValidatorTests()
    {
        _validator = new SaveCommitmentCommandValidator();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Validate_CommitmentId_RequiresNonEmptyGuid(bool useNewGuid)
    {
        var request = new SaveCommitmentRequest
        {
            Commitment = new CommitmentDto
            {
                CommitmentId = useNewGuid ? Guid.NewGuid() : default(Guid)
            }
        };

        var result = _validator.Validate(request);

        result.IsValid.Should().Be(useNewGuid);
    }

    [Fact]
    public void Validate_NewGuid_Passes()
    {
        var request = new SaveCommitmentRequest
        {
            Commitment = new CommitmentDto
            {
                CommitmentId = Guid.NewGuid(),
                BehaviourId = Guid.NewGuid(),
                ProfileId = Guid.NewGuid()
            }
        };

        var result = _validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_DefaultGuid_Fails()
    {
        var request = new SaveCommitmentRequest
        {
            Commitment = new CommitmentDto
            {
                CommitmentId = default(Guid)
            }
        };

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }
}
