using Commitments.Shared;

namespace Identity.Domain.ProfileAggregate;

public class Profile : BaseEntity
{
    public Guid ProfileId { get; set; }
    public Guid UserId { get; set; }
}
