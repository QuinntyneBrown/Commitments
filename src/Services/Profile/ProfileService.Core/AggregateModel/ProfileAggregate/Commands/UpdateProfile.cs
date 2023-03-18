// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace ProfileService.Core.AggregateModel.ProfileAggregate.Commands;

public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileRequestValidator()
    {

        RuleFor(x => x.ProfileId).NotEqual(default(Guid));
        RuleFor(x => x.Name).NotNull();
        RuleFor(x => x.Email).NotNull();

    }

}


public class UpdateProfileRequest : IRequest<UpdateProfileResponse>
{
    public Guid ProfileId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}


public class UpdateProfileResponse
{
    public required ProfileDto Profile { get; set; }
}


public class UpdateProfileRequestHandler : IRequestHandler<UpdateProfileRequest, UpdateProfileResponse>
{
    private readonly IProfileServiceDbContext _context;

    private readonly ILogger<UpdateProfileRequestHandler> _logger;

    public UpdateProfileRequestHandler(ILogger<UpdateProfileRequestHandler> logger, IProfileServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<UpdateProfileResponse> Handle(UpdateProfileRequest request, CancellationToken cancellationToken)
    {
        var profile = await _context.Profiles.SingleAsync(x => x.ProfileId == request.ProfileId);

        profile.Name = request.Name;
        profile.Email = request.Email;

        await _context.SaveChangesAsync(cancellationToken);

        return new()
        {
            Profile = profile.ToDto()
        };

    }

}



