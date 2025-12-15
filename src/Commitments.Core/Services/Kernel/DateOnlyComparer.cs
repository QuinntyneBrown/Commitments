// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Commitments.Core.Services.Kernel;

public class DateOnlyComparer : IEqualityComparer<DateOnly>
{
    public bool Equals(DateOnly x, DateOnly y)
    {
        return x.CompareTo(y) == 0;
    }

    public int GetHashCode(DateOnly obj)
    {
        return obj.GetHashCode();
    }
}
