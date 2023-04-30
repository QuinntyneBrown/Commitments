// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using IdentityService.Core.AggregateModel.UserAggregate;
using IdentityService.Core.Messages;
using Messaging;
using Security;

namespace IdentityService.Core.MessageHandlers;

public class UserCreateMessageHandler : IRequestHandler<UserCreateMessage>
{
    private readonly ILogger<UserCreateMessageHandler> _logger;
    private readonly IIdentityServiceDbContext _identityServiceDbContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IServiceBusMessageSender _serviceBusMessageSender;

    public UserCreateMessageHandler(
        ILogger<UserCreateMessageHandler> logger,
        IIdentityServiceDbContext identityServiceDbContext,
        IPasswordHasher passwordHasher,
        IServiceBusMessageSender serviceBusMessageSender)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _identityServiceDbContext = identityServiceDbContext ?? throw new ArgumentNullException(nameof(identityServiceDbContext));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _serviceBusMessageSender = serviceBusMessageSender ?? throw new ArgumentNullException(nameof(serviceBusMessageSender));
    }

    public async Task Handle(UserCreateMessage message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Message Handled: {message}", message);

        var user = new User(message.Username, message.Password, _passwordHasher);

        var existingUser = await _identityServiceDbContext.Users.SingleOrDefaultAsync(x => x.Username == message.Username, cancellationToken);

        if (existingUser == null)
        {
            _identityServiceDbContext.Users.Add(user);

            await _identityServiceDbContext.SaveChangesAsync(cancellationToken);

            await _serviceBusMessageSender.Send(new UserCreatedMessage()
            {
                Username = message.Username,
                Email = message.Username,
                Name = message.Username
            });
        }

    }
}
