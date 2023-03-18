// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace DashboardService.Core.AggregateModel.UserAggregate.Commands;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest> { }

public class CreateUserRequest : IRequest<CreateUserResponse> { }

public class CreateUserResponse : ResponseBase
{
    public UserDto User { get; set; }
}


public class CreateUserRequestHandler : IRequestHandler<CreateUserRequest, CreateUserResponse>
{
    private readonly ILogger<CreateUserRequestHandler> _logger;

    private readonly IDashboardServiceDbContext _context;

    public CreateUserRequestHandler(ILogger<CreateUserRequestHandler> logger, IDashboardServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<CreateUserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var user = new User();

        _context.Users.Add(user);


        await _context.SaveChangesAsync(cancellationToken);

        return new()
        {
            User = user.ToDto()
        };

    }

}



