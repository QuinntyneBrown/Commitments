using Commitments.Shared;
using FluentAssertions;
using Xunit;

namespace Commitments.Shared.Tests;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("Hello", "hello")]
    [InlineData("", "")]
    [InlineData("a", "a")]
    [InlineData("ABC", "aBC")]
    [InlineData("already", "already")]
    public void ToCamelCase_ShouldConvertCorrectly(string input, string expected)
    {
        input.ToCamelCase().Should().Be(expected);
    }

    [Fact]
    public void ToCamelCase_Null_ReturnsNull()
    {
        string? value = null;
        value!.ToCamelCase().Should().BeNull();
    }

    [Theory]
    [InlineData("Hello World", "hello-world")]
    [InlineData("", "")]
    [InlineData("  ", "")]
    [InlineData("Hello  World", "hello-world")]
    [InlineData("Hello@World!", "helloworld")]
    [InlineData("simple", "simple")]
    [InlineData("UPPER CASE", "upper-case")]
    public void GenerateSlug_ShouldConvertCorrectly(string input, string expected)
    {
        input.GenerateSlug().Should().Be(expected);
    }
}
