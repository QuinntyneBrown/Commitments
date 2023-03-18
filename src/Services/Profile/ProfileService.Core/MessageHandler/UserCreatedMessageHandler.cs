// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using ProfileService.Core.AggregateModel.ProfileAggregate;
using ProfileService.Core.Messages;

namespace ProfileService.Core.MessageHandler;

public class UserCreatedMessageHandler: IRequestHandler<UserCreatedMessage>
{
    private readonly ILogger<UserCreatedMessageHandler> _logger;
    private readonly IProfileServiceDbContext _context;

    public UserCreatedMessageHandler(
        ILogger<UserCreatedMessageHandler> logger, IProfileServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task Handle(UserCreatedMessage message,CancellationToken cancellationToken)
    {
        _logger.LogInformation("Message Handled: {message}", message);

        var profile = new Profile()
        {
            Name = message.Username,
            Email = message.Username
        };

        _context.Profiles.Add(profile);

        await _context.SaveChangesAsync(cancellationToken);

    }

}


