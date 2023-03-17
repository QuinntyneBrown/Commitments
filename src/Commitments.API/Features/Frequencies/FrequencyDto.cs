// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Api.Features.FrequencyTypes;
using Commitments.Core.AggregateModel;


namespace Commitments.Api.Features.Frequencies;

public class FrequencyDto
{        
    public int FrequencyId { get; set; }
    public int Frequency { get; set; }
    public int FrequencyTypeId { get; set; }
    public FrequencyTypeDto FrequencyType { get; set; }

    public static FrequencyDto FromFrequency(Frequency frequency)
    {
        var model = new FrequencyDto();
        model.FrequencyId = frequency.FrequencyId;
        model.Frequency = frequency.Frequency;
        model.FrequencyTypeId = frequency.FrequencyTypeId;
        model.FrequencyType = FrequencyTypeDto.FromFrequencyType(frequency.FrequencyType);
        return model;
    }
}

