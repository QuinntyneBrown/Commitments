// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Entities;
using System;
using Xunit;


namespace UnitTests.Core;

public class UserTests
{
    [Fact]
    public void ShouldHaveSaltByDefault() {
        var user = new User();
        Assert.NotEqual(default(Byte[]),user.Salt);
    }
}

