using Commitments.Shared;

namespace Commitments.Domain.TagAggregate;

public class Tag : BaseEntity
{
    public Guid TagId { get; set; }
    public Guid ProfileId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
}
