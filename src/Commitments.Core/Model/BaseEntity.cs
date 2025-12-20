// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Interfaces;


namespace Commitments.Core.Model;

public class BaseEntity : ILoggable
{
    public DateTime CreatedOn { get; set; }
    public DateTime LastModifiedOn { get; set; }
    public bool IsDeleted { get; set; }
}