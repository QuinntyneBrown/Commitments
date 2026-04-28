using Commitments.Data;
using Commitments.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Commitments.Features.Tag;

public class RemoveTagRequest : IRequest
{
    public Guid TagId { get; set; }
}

public class RemoveTagCommandHandler : IRequestHandler<RemoveTagRequest>
{
    private readonly ICommitmentsDbContext _context;
    private readonly IEventBus _bus;

    public RemoveTagCommandHandler(ICommitmentsDbContext context, IEventBus bus)
    {
        _context = context;
        _bus = bus;
    }

    public async Task Handle(RemoveTagRequest request, CancellationToken cancellationToken)
    {
        var referenceCount = _context.NoteTags.Count(nt => nt.TagId == request.TagId);
        if (referenceCount > 0)
        {
            throw new BadHttpRequestException($"Cannot delete: referenced by {referenceCount} note(s)", 400);
        }

        var tag = await _context.Tags.FindAsync(new object?[] { request.TagId }, cancellationToken);
        if (tag is null) return;

        var profileId = tag.ProfileId;
        _context.Tags.Remove(tag);
        await _context.SaveChangesAsync(cancellationToken);

        await _bus.PublishAsync(new TagRemovedEvent { TagId = request.TagId, ProfileId = profileId }, cancellationToken);
    }
}
