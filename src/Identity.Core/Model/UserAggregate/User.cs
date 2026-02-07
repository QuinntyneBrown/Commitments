using Commitments.Shared;

namespace Identity.Core.Model.UserAggregate;

public class User : BaseEntity
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
}
