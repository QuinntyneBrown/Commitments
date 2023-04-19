// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace NoteService.Core.AggregateModel.TagAggregate;

public static class TagExtensions
{
    public static TagDto ToDto(this Tag tag)
    {
        return new TagDto
        {
            TagId = tag.TagId,
            Name = tag.Name,
            Slug = tag.Slug,
        };

    }

    public async static Task<List<TagDto>> ToDtosAsync(this IQueryable<Tag> tags, CancellationToken cancellationToken)
    {
        return await tags.Select(x => x.ToDto()).ToListAsync(cancellationToken);
    }

}