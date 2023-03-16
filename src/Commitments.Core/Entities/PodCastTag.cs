
namespace Commitments.Core.Entities;

public class PodCastTag
{
    public int PodCastTagId { get; set; }
    public int PodCastId { get; set; }
    public int TagId { get; set; }
    public PodCast PodCast { get; set; }
    public Tag Tag { get; set; }
}
