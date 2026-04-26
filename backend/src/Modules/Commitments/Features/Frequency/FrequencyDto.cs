// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Domain.FrequencyAggregate;
using Commitments.Domain.FrequencyTypeAggregate;
using Commitments.Features.FrequencyType;
using FrequencyEntity = Commitments.Domain.FrequencyAggregate.Frequency;

namespace Commitments.Features.Frequency;

public class FrequencyDto
{
    public Guid FrequencyId { get; set; }
    public Guid Frequency { get; set; }
    public Guid FrequencyTypeId { get; set; }
    public FrequencyTypeDto FrequencyType { get; set; }

    public static FrequencyDto FromFrequency(FrequencyEntity frequency)
    {
        var model = new FrequencyDto();
        model.FrequencyId = frequency.FrequencyId;
        model.Frequency = frequency.Frequency;
        model.FrequencyTypeId = frequency.FrequencyTypeId;
        model.FrequencyType = FrequencyTypeDto.FromFrequencyType(frequency.FrequencyType);
        return model;
    }
}