namespace Commitments.Core.Entities
{
    public class CardLayout: BaseEntity
    {
        public int CardLayoutId { get; set; }           
		public string Name { get; set; }
        public string Description { get; set; }
    }
}
