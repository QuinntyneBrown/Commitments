using Commitments.Features.Activity;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Commands;

public class SaveActivityValidatorTests
{
    private readonly SaveActivityCommandValidator _validator = new();

    [Fact]
    public void Validate_FailsWhenBehaviourIdIsEmpty(/* design 10 Slice C / L2-040 */)
    {
        var request = new SaveActivityRequest
        {
            Activity = new ActivityDto
            {
                ActivityId = Guid.NewGuid(),
                BehaviourId = Guid.Empty
            }
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor("Activity.BehaviourId");
    }

    [Fact]
    public void Validate_PassesWhenBehaviourIdIsSet()
    {
        var request = new SaveActivityRequest
        {
            Activity = new ActivityDto
            {
                ActivityId = Guid.NewGuid(),
                BehaviourId = Guid.NewGuid()
            }
        };

        var result = _validator.TestValidate(request);

        result.IsValid.Should().BeTrue();
    }
}
