using Commitments.Api.Features.FrequencyTypes;
using Commitments.Core.Entities;

namespace Commitments.Api.Features.Frequencies
{
    public class FrequencyApiModel
    {        
        public int FrequencyId { get; set; }
        public int Frequency { get; set; }
        public int FrequencyTypeId { get; set; }
        public FrequencyTypeApiModel FrequencyType { get; set; }

        public static FrequencyApiModel FromFrequency(Frequency frequency)
        {
            var model = new FrequencyApiModel();
            model.FrequencyId = frequency.FrequencyId;
            model.Frequency = frequency.Frequency;
            model.FrequencyTypeId = frequency.FrequencyTypeId;
            model.FrequencyType = FrequencyTypeApiModel.FromFrequencyType(frequency.FrequencyType);
            return model;
        }
    }
}
