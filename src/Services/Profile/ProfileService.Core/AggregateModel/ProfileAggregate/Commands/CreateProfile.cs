// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace ProfileService.Core.AggregateModel.ProfileAggregate.Commands;

public class CreateProfileRequestValidator : AbstractValidator<CreateProfileRequest>
{
    public CreateProfileRequestValidator()
    {

        RuleFor(x => x.Name).NotNull();
        RuleFor(x => x.Email).NotNull();

    }

}


public class CreateProfileRequest : IRequest<CreateProfileResponse>
{
    public string Name { get; set; }
    public string Email { get; set; }
}


public class CreateProfileResponse
{
    public required ProfileDto Profile { get; set; }
}


public class CreateProfileRequestHandler : IRequestHandler<CreateProfileRequest, CreateProfileResponse>
{
    private readonly IProfileServiceDbContext _context;

    private readonly ILogger<CreateProfileRequestHandler> _logger;

    public CreateProfileRequestHandler(ILogger<CreateProfileRequestHandler> logger, IProfileServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<CreateProfileResponse> Handle(CreateProfileRequest request, CancellationToken cancellationToken)
    {
        var profile = new Profile();

        _context.Profiles.Add(profile);

        profile.Name = request.Name;
        profile.Email = request.Email;

        await _context.SaveChangesAsync(cancellationToken);

        return new()
        {
            Profile = profile.ToDto()
        };

    }

}



