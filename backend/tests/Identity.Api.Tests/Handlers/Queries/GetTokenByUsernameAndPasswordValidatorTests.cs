using FluentAssertions;
using FluentValidation.TestHelper;
using Identity.Features.User;
using Xunit;

namespace Identity.Api.Tests.Handlers.Queries;

public class GetTokenByUsernameAndPasswordValidatorTests
{
    private readonly GetTokenByUsernameAndPasswordRequestValidator _validator = new();

    [Fact]
    public void Validate_ShouldFail_WhenUsernameIsEmpty()
    {
        var result = _validator.TestValidate(new GetTokenByUsernameAndPasswordRequest
        {
            Username = string.Empty,
            Password = "password"
        });

        result.ShouldHaveValidationErrorFor(r => r.Username);
    }

    [Fact]
    public void Validate_ShouldFail_WhenPasswordIsEmpty()
    {
        var result = _validator.TestValidate(new GetTokenByUsernameAndPasswordRequest
        {
            Username = "quinntynebrown@gmail.com",
            Password = string.Empty
        });

        result.ShouldHaveValidationErrorFor(r => r.Password);
    }

    [Fact]
    public void Validate_ShouldPass_WhenBothFieldsAreProvided()
    {
        var result = _validator.TestValidate(new GetTokenByUsernameAndPasswordRequest
        {
            Username = "quinntynebrown@gmail.com",
            Password = "password"
        });

        result.IsValid.Should().BeTrue();
    }
}
