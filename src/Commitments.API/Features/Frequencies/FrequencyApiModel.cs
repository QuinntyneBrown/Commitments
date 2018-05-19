using Commitments.Core.Entities;

namespace Commitments.API.Features.Frequencies
{
    public class FrequencyApiModel
    {        
        public int FrequencyId { get; set; }
        public int Frequency { get; set; }
        public int FrequencyTypeId { get; set; }
        public static FrequencyApiModel FromFrequency(Frequency frequency)
        {
            var model = new FrequencyApiModel();
            model.FrequencyId = frequency.FrequencyId;
            model.Frequency = frequency.Frequency;
            model.FrequencyTypeId = frequency.FrequencyTypeId;
            return model;
        }
    }
}
