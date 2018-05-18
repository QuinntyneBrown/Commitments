namespace Commitments.Core.Entities
{
    public class Behaviour: BaseEntity
    {
        public int BehaviourId { get; set; }           
		public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public int? BehaviourTypeId { get; set; }
        public BehaviourType BehaviourType { get; set; }
    }
}
