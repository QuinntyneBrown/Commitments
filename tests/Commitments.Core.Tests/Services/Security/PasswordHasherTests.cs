// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Services.Security;
using FluentAssertions;
using Xunit;

namespace Commitments.Core.Tests.Services.Security;

public class PasswordHasherTests
{
    private readonly PasswordHasher _passwordHasher;

    public PasswordHasherTests()
    {
        _passwordHasher = new PasswordHasher();
    }

    [Fact]
    public void HashPassword_ShouldReturnNonEmptyString()
    {
        // Arrange
        var salt = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        var password = "testPassword123";

        // Act
        var result = _passwordHasher.HashPassword(salt, password);

        // Assert
        result.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void HashPassword_ShouldReturnBase64String()
    {
        // Arrange
        var salt = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        var password = "testPassword123";

        // Act
        var result = _passwordHasher.HashPassword(salt, password);
        var isValidBase64 = !string.IsNullOrWhiteSpace(result);
        try
        {
            Convert.FromBase64String(result);
        }
        catch
        {
            isValidBase64 = false;
        }

        // Assert
        isValidBase64.Should().BeTrue();
    }

    [Fact]
    public void HashPassword_ShouldReturnSameHashForSameInput()
    {
        // Arrange
        var salt = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        var password = "testPassword123";

        // Act
        var result1 = _passwordHasher.HashPassword(salt, password);
        var result2 = _passwordHasher.HashPassword(salt, password);

        // Assert
        result1.Should().Be(result2);
    }

    [Fact]
    public void HashPassword_ShouldReturnDifferentHashForDifferentSalt()
    {
        // Arrange
        var salt1 = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        var salt2 = new byte[] { 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
        var password = "testPassword123";

        // Act
        var result1 = _passwordHasher.HashPassword(salt1, password);
        var result2 = _passwordHasher.HashPassword(salt2, password);

        // Assert
        result1.Should().NotBe(result2);
    }

    [Fact]
    public void HashPassword_ShouldReturnDifferentHashForDifferentPassword()
    {
        // Arrange
        var salt = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        var password1 = "testPassword123";
        var password2 = "testPassword456";

        // Act
        var result1 = _passwordHasher.HashPassword(salt, password1);
        var result2 = _passwordHasher.HashPassword(salt, password2);

        // Assert
        result1.Should().NotBe(result2);
    }

    [Fact]
    public void HashPassword_ShouldHandleEmptyPassword()
    {
        // Arrange
        var salt = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        var password = "";

        // Act
        var result = _passwordHasher.HashPassword(salt, password);

        // Assert
        result.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void HashPassword_ShouldHandleLongPassword()
    {
        // Arrange
        var salt = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        var password = new string('a', 1000);

        // Act
        var result = _passwordHasher.HashPassword(salt, password);

        // Assert
        result.Should().NotBeNullOrEmpty();
    }
}
