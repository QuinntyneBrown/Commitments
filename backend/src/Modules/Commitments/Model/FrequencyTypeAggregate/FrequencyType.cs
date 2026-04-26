// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Commitments.Core.Model.FrequencyTypeAggregate;

public class FrequencyType : BaseEntity
{
    public Guid FrequencyTypeId { get; set; }
    public string Name { get; set; }
}