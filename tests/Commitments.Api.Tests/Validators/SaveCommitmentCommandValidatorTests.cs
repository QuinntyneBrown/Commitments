using Commitments.Api.Features.Commitment;
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
    public void Validate_CommitmentId_NotNullOnGuidAlwaysPasses(bool useNewGuid)
    {
        // Note: SaveCommitmentCommandValidator uses .NotNull() on Guid (a value type).
        // Because value types can never be null, this validation always passes.
        // This is a known bug where NotNull on value types is a no-op.
        var request = new SaveCommitmentRequest
        {
            Commitment = new CommitmentDto
            {
                CommitmentId = useNewGuid ? Guid.NewGuid() : default(Guid)
            }
        };

        var result = _validator.Validate(request);

        // Both cases pass because NotNull on a Guid (value type) always succeeds
        result.IsValid.Should().BeTrue();
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
    public void Validate_DefaultGuid_StillPassesDueToNotNullBug()
    {
        // This test documents the known bug: NotNull() on value types always passes
        var request = new SaveCommitmentRequest
        {
            Commitment = new CommitmentDto
            {
                CommitmentId = default(Guid)
            }
        };

        var result = _validator.Validate(request);

        // NotNull on a Guid value type always passes -- this is a known issue
        result.IsValid.Should().BeTrue();
    }
}
