// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Model.FrequencyAggregate;
using Commitments.Core.Model.FrequencyTypeAggregate;
using Commitments.Api.Features.FrequencyType;
using FrequencyEntity = Commitments.Core.Model.FrequencyAggregate.Frequency;

namespace Commitments.Api.Features.Frequency;

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