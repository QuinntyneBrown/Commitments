using Commitments.Shared;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;
using Xunit;

namespace Commitments.Shared.Tests;

public class ValidationTestRequest : IRequest<ValidationTestResponse>
{
    public string Value { get; set; } = "";
}

public class ValidationTestResponse : ResponseBase
{
    public string Result { get; set; } = "";
}

public class ValidationBehaviorTests
{
    [Fact]
    public async Task Handle_NoValidators_CallsNext()
    {
        var validators = Enumerable.Empty<IValidator<ValidationTestRequest>>();
        var behavior = new ValidationBehavior<ValidationTestRequest, ValidationTestResponse>(validators);
        var nextCalled = false;

        var result = await behavior.Handle(
            new ValidationTestRequest(),
            () => { nextCalled = true; return Task.FromResult(new ValidationTestResponse { Result = "ok" }); },
            CancellationToken.None);

        nextCalled.Should().BeTrue();
        result.Result.Should().Be("ok");
    }

    [Fact]
    public async Task Handle_ValidRequest_CallsNext()
    {
        var mockValidator = new Mock<IValidator<ValidationTestRequest>>();
        mockValidator.Setup(v => v.Validate(It.IsAny<ValidationContext<ValidationTestRequest>>()))
            .Returns(new ValidationResult());

        var behavior = new ValidationBehavior<ValidationTestRequest, ValidationTestResponse>(new[] { mockValidator.Object });
        var nextCalled = false;

        var result = await behavior.Handle(
            new ValidationTestRequest { Value = "valid" },
            () => { nextCalled = true; return Task.FromResult(new ValidationTestResponse { Result = "ok" }); },
            CancellationToken.None);

        nextCalled.Should().BeTrue();
        result.Result.Should().Be("ok");
    }

    [Fact]
    public async Task Handle_InvalidRequest_ReturnsResponseWithErrors_DoesNotCallNext()
    {
        var mockValidator = new Mock<IValidator<ValidationTestRequest>>();
        mockValidator.Setup(v => v.Validate(It.IsAny<ValidationContext<ValidationTestRequest>>()))
            .Returns(new ValidationResult(new[] { new ValidationFailure("Value", "Value is required") }));

        var behavior = new ValidationBehavior<ValidationTestRequest, ValidationTestResponse>(new[] { mockValidator.Object });
        var nextCalled = false;

        var result = await behavior.Handle(
            new ValidationTestRequest(),
            () => { nextCalled = true; return Task.FromResult(new ValidationTestResponse { Result = "ok" }); },
            CancellationToken.None);

        nextCalled.Should().BeFalse();
        result.Errors.Should().Contain("Value is required");
    }
}
