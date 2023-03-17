using Commitments.Core.AggregateModel;

public class PodCast: BaseEntity
{	
	public int PodCastId { get; set; }
	public string Url { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
}
