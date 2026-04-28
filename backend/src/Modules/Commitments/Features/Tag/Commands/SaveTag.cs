using Commitments.Data;
using Commitments.Shared;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TagEntity = Commitments.Domain.TagAggregate.Tag;

namespace Commitments.Features.Tag;

public class SaveTagCommandValidator : AbstractValidator<SaveTagRequest>
{
    public SaveTagCommandValidator()
    {
        RuleFor(r => r.Tag.Name).NotEmpty();
    }
}

public class SaveTagRequest : IRequest<SaveTagResponse>
{
    public TagDto Tag { get; set; } = new();
}

public class SaveTagResponse
{
    public Guid TagId { get; set; }
    public string Slug { get; set; } = string.Empty;
}

public class SaveTagCommandHandler : IRequestHandler<SaveTagRequest, SaveTagResponse>
{
    private readonly ICommitmentsDbContext _context;
    private readonly IEventBus _bus;

    public SaveTagCommandHandler(ICommitmentsDbContext context, IEventBus bus)
    {
        _context = context;
        _bus = bus;
    }

    public async Task<SaveTagResponse> Handle(SaveTagRequest request, CancellationToken cancellationToken)
    {
        var tag = await _context.Tags.FindAsync(new object?[] { request.Tag.TagId }, cancellationToken);
        if (tag is null) _context.Tags.Add(tag = new TagEntity { ProfileId = request.Tag.ProfileId });

        tag.Name = request.Tag.Name;
        tag.Slug = await UniqueSlug(request.Tag.Name.GenerateSlug(), tag.ProfileId, tag.TagId, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        await _bus.PublishAsync(new TagSavedEvent { TagId = tag.TagId, ProfileId = tag.ProfileId }, cancellationToken);

        return new SaveTagResponse { TagId = tag.TagId, Slug = tag.Slug };
    }

    private async Task<string> UniqueSlug(string baseSlug, Guid profileId, Guid tagId, CancellationToken ct)
    {
        var taken = await _context.Tags
            .Where(t => t.ProfileId == profileId && t.TagId != tagId)
            .Select(t => t.Slug)
            .ToListAsync(ct);

        if (!taken.Contains(baseSlug)) return baseSlug;
        for (var i = 2; ; i++)
        {
            var candidate = $"{baseSlug}-{i}";
            if (!taken.Contains(candidate)) return candidate;
        }
    }
}
