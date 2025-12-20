// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Services.Kernel;
using FluentAssertions;
using Xunit;

namespace Commitments.Core.Tests.Services.Kernel;

public class StringExtensionsTests
{
    [Fact]
    public void ToCamelCase_ShouldConvertPascalCaseToCamelCase()
    {
        // Arrange
        var input = "HelloWorld";

        // Act
        var result = input.ToCamelCase();

        // Assert
        result.Should().Be("helloWorld");
    }

    [Fact]
    public void ToCamelCase_ShouldReturnSameStringWhenAlreadyCamelCase()
    {
        // Arrange
        var input = "helloWorld";

        // Act
        var result = input.ToCamelCase();

        // Assert
        result.Should().Be("helloWorld");
    }

    [Fact]
    public void ToCamelCase_ShouldHandleSingleUppercaseCharacter()
    {
        // Arrange
        var input = "A";

        // Act
        var result = input.ToCamelCase();

        // Assert
        result.Should().Be("a");
    }

    [Fact]
    public void ToCamelCase_ShouldHandleSingleLowercaseCharacter()
    {
        // Arrange
        var input = "a";

        // Act
        var result = input.ToCamelCase();

        // Assert
        result.Should().Be("a");
    }

    [Fact]
    public void ToCamelCase_ShouldReturnEmptyStringForEmptyInput()
    {
        // Arrange
        var input = "";

        // Act
        var result = input.ToCamelCase();

        // Assert
        result.Should().Be("");
    }

    [Fact]
    public void ToCamelCase_ShouldReturnNullForNullInput()
    {
        // Arrange
        string? input = null;

        // Act
        var result = input.ToCamelCase();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void ToCamelCase_ShouldHandleAllUppercase()
    {
        // Arrange
        var input = "HELLO";

        // Act
        var result = input.ToCamelCase();

        // Assert
        result.Should().Be("hELLO");
    }

    [Fact]
    public void ToCamelCase_ShouldHandleAllLowercase()
    {
        // Arrange
        var input = "hello";

        // Act
        var result = input.ToCamelCase();

        // Assert
        result.Should().Be("hello");
    }

    [Fact]
    public void ToCamelCase_ShouldHandleStringWithNumbers()
    {
        // Arrange
        var input = "Hello123World";

        // Act
        var result = input.ToCamelCase();

        // Assert
        result.Should().Be("hello123World");
    }

    [Fact]
    public void ToCamelCase_ShouldHandleStringWithSpecialCharacters()
    {
        // Arrange
        var input = "Hello_World";

        // Act
        var result = input.ToCamelCase();

        // Assert
        result.Should().Be("hello_World");
    }
}
