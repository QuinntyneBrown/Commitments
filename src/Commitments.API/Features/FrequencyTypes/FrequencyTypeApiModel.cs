using Commitments.Core.Entities;

namespace Commitments.API.Features.FrequencyTypes
{
    public class FrequencyTypeApiModel
    {        
        public int FrequencyTypeId { get; set; }
        public string Name { get; set; }

        public static FrequencyTypeApiModel FromFrequencyType(FrequencyType frequencyType)
        {
            var model = new FrequencyTypeApiModel();
            model.FrequencyTypeId = frequencyType.FrequencyTypeId;
            model.Name = frequencyType.Name;
            return model;
        }
    }
}
