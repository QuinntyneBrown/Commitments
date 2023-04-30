// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using IdentityService.Core.AggregateModel.UserAggregate;
using IdentityService.Core.Messages;
using Security;

namespace IdentityService.Core.MessageHandlers;

public class UserCreateMessageHandler: IRequestHandler<UserCreateMessage>
{
    private readonly ILogger<UserCreateMessageHandler> _logger;
    private readonly IIdentityServiceDbContext _identityServiceDbContext;
    private readonly IPasswordHasher _passwordHasher;

    public UserCreateMessageHandler(
        ILogger<UserCreateMessageHandler> logger, 
        IIdentityServiceDbContext identityServiceDbContext,
        IPasswordHasher passwordHasher)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _identityServiceDbContext = identityServiceDbContext ?? throw new ArgumentNullException(nameof(identityServiceDbContext));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));    
    }

    public async Task Handle(UserCreateMessage message,CancellationToken cancellationToken)
    {
        _logger.LogInformation("Message Handled: {message}", message);

        var user = new User(message.Username, message.Password, _passwordHasher);

        _identityServiceDbContext.Users.Add(user);

        await _identityServiceDbContext.SaveChangesAsync(cancellationToken);
    }
}


