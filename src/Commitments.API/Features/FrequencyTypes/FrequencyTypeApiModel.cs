using Commitments.Core.Entities;


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
