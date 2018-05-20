namespace Commitments.Core.Entities
{
    public class BaseFrequency: BaseEntity {
        public int Frequency { get; set; }
    }

    public class Frequency: BaseFrequency
    {
        public int FrequencyId { get; set; }
        public int FrequencyTypeId { get; set; }
        public bool IsDesirable { get; set; }
        public FrequencyType FrequencyType { get; set; }
    }
}
