// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Commitments.Core.Interfaces;

public interface ILoggable
{
    DateTime CreatedOn { get; set; }
    DateTime LastModifiedOn { get; set; }
    bool IsDeleted { get; set; }
}