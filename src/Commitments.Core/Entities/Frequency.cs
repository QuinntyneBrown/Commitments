namespace Commitments.Core.Entities
{
    public class FrequencyBase {
        public int Frequency { get; set; }
    }

    public class Frequency: FrequencyBase
    {
        public int FrequencyId { get; set; }
        public int FrequencyTypeId { get; set; }
        public FrequencyType FrequencyType { get; set; }
    }
}
