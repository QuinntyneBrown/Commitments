using TagEntity = Commitments.Domain.TagAggregate.Tag;

namespace Commitments.Features.Tag;

public class TagDto
{
    public Guid TagId { get; set; }
    public Guid ProfileId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

    public static TagDto FromTag(TagEntity tag) => new()
    {
        TagId = tag.TagId,
        ProfileId = tag.ProfileId,
        Name = tag.Name,
        Slug = tag.Slug
    };
}
