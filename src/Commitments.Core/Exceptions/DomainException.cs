// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;


namespace Commitments.Core.Exceptions;

public class DomainException: Exception
{
    public int Code { get; set; } = 0;
}

