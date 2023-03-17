// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel;


namespace Commitments.Api.Features.FrequencyTypes;

public class FrequencyTypeDto
{        
    public int FrequencyTypeId { get; set; }
    public string Name { get; set; }

    public static FrequencyTypeDto FromFrequencyType(FrequencyType frequencyType)
    {
        var model = new FrequencyTypeDto();
        model.FrequencyTypeId = frequencyType.FrequencyTypeId;
        model.Name = frequencyType.Name;
        return model;
    }
}

