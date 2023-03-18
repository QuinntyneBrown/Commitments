// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using IdentityService.Core.Messages;
using Messaging;
using Security;

namespace IdentityService.Core.AggregateModel.UserAggregate.Commands;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest> {
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Username).NotNull().NotEmpty();
    }
}

public class CreateUserRequest : IRequest<CreateUserResponse> {

    public string Username { get; set; }
}

public class CreateUserResponse : ResponseBase
{
    public required UserDto User { get; set; }
}


public class CreateUserRequestHandler : IRequestHandler<CreateUserRequest, CreateUserResponse>
{
    private readonly ILogger<CreateUserRequestHandler> _logger;
    private readonly IServiceBusMessageSender _serviceBusMessageSender;
    private readonly IIdentityServiceDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserRequestHandler(ILogger<CreateUserRequestHandler> logger, IIdentityServiceDbContext context, IPasswordHasher passwordHasher, IServiceBusMessageSender serviceBusMessageSender)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _serviceBusMessageSender = serviceBusMessageSender ?? throw new ArgumentNullException(nameof(serviceBusMessageSender));
    }

    public async Task<CreateUserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var user = new User(request.Username,"Default", _passwordHasher);

            _context.Users.Add(user);

            await _context.SaveChangesAsync(cancellationToken);

            await _serviceBusMessageSender.Send(new UserCreatedMessage()
            {
                Name = user.Username,
                Username = user.Username,
                Email = user.Username
            });

            return new()
            {
                User = user.ToDto()
            };
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }
}