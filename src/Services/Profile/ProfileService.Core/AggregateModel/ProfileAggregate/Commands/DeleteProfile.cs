// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace ProfileService.Core.AggregateModel.ProfileAggregate.Commands;

public class DeleteProfileRequestValidator : AbstractValidator<DeleteProfileRequest>
{
    public DeleteProfileRequestValidator()
    {

        RuleFor(x => x.ProfileId).NotEqual(default(Guid));

    }

}


public class DeleteProfileRequest : IRequest<DeleteProfileResponse>
{
    public Guid ProfileId { get; set; }
}


public class DeleteProfileResponse
{
    public required ProfileDto Profile { get; set; }
}


public class DeleteProfileRequestHandler : IRequestHandler<DeleteProfileRequest, DeleteProfileResponse>
{
    private readonly IProfileServiceDbContext _context;

    private readonly ILogger<DeleteProfileRequestHandler> _logger;

    public DeleteProfileRequestHandler(ILogger<DeleteProfileRequestHandler> logger, IProfileServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<DeleteProfileResponse> Handle(DeleteProfileRequest request, CancellationToken cancellationToken)
    {
        var profile = await _context.Profiles.FindAsync(request.ProfileId);

        _context.Profiles.Remove(profile);

        await _context.SaveChangesAsync(cancellationToken);

        return new()
        {
            Profile = profile.ToDto()
        };
    }

}



