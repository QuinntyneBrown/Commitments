// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Domain.FrequencyTypeAggregate;
using FrequencyTypeEntity = Commitments.Domain.FrequencyTypeAggregate.FrequencyType;

namespace Commitments.Features.FrequencyType;

public class FrequencyTypeDto
{
    public Guid FrequencyTypeId { get; set; }
    public string Name { get; set; }

    public static FrequencyTypeDto FromFrequencyType(FrequencyTypeEntity frequencyType)
    {
        var model = new FrequencyTypeDto();
        model.FrequencyTypeId = frequencyType.FrequencyTypeId;
        model.Name = frequencyType.Name;
        return model;
    }
}