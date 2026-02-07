using Commitments.Shared;
using Identity.Core;
using MediatR;

namespace Identity.Api.Features.Profile;

public class CreateProfileRequest : IRequest<CreateProfileResponse>
{
    public ProfileDto Profile { get; set; }
}

public class CreateProfileResponse : ResponseBase
{
    public ProfileDto Profile { get; set; }
}

public class CreateProfileRequestHandler : IRequestHandler<CreateProfileRequest, CreateProfileResponse>
{
    private readonly IIdentityDbContext _context;
    private readonly IEventBus _eventBus;

    public CreateProfileRequestHandler(IIdentityDbContext context, IEventBus eventBus)
    {
        _context = context;
        _eventBus = eventBus;
    }

    public async Task<CreateProfileResponse> Handle(CreateProfileRequest request, CancellationToken cancellationToken)
    {
        var profile = new Identity.Core.Model.ProfileAggregate.Profile();

        _context.Profiles.Add(profile);
        await _context.SaveChangesAsync(cancellationToken);

        await _eventBus.PublishAsync(new ProfileCreatedEvent { ProfileId = profile.ProfileId }, cancellationToken);

        return new CreateProfileResponse { Profile = profile.ToDto() };
    }
}
