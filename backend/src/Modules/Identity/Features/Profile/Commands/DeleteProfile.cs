using Commitments.Shared;
using Identity.Core;
using MediatR;

namespace Identity.Api.Features.Profile;

public class DeleteProfileRequest : IRequest<DeleteProfileResponse>
{
    public Guid ProfileId { get; set; }
}

public class DeleteProfileResponse : ResponseBase { }

public class DeleteProfileRequestHandler : IRequestHandler<DeleteProfileRequest, DeleteProfileResponse>
{
    private readonly IIdentityDbContext _context;
    private readonly IEventBus _eventBus;

    public DeleteProfileRequestHandler(IIdentityDbContext context, IEventBus eventBus)
    {
        _context = context;
        _eventBus = eventBus;
    }

    public async Task<DeleteProfileResponse> Handle(DeleteProfileRequest request, CancellationToken cancellationToken)
    {
        var profile = await _context.Profiles.FindAsync(request.ProfileId);
        if (profile != null)
        {
            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync(cancellationToken);
            await _eventBus.PublishAsync(new ProfileDeletedEvent { ProfileId = request.ProfileId }, cancellationToken);
        }
        return new DeleteProfileResponse();
    }
}
