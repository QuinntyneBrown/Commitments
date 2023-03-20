// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Kernel;

public class DateOnlyComparer : ValueComparer<DateOnly>
{
    public DateOnlyComparer() : base(
        (d1, d2) => d1.DayNumber == d2.DayNumber,
        d => d.GetHashCode())
    {
    }
}

