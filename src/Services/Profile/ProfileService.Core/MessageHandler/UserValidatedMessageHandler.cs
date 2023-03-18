// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Messaging;
using ProfileService.Core.Messages;

namespace ProfileService.Core.MessageHandler;

public class UserValidatedMessageHandler: IRequestHandler<UserValidatedMessage>
{
    private readonly ILogger<UserValidatedMessage> _logger;
    private readonly IProfileServiceDbContext _context;
    private readonly IServiceBusMessageSender _serviceBusMessageSender;

    public UserValidatedMessageHandler(ILogger<UserValidatedMessage> logger){
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(UserValidatedMessage message,CancellationToken cancellationToken)
    {
        _logger.LogInformation("Message Handled: {message}", message);

        var profile = _context.Profiles.Single(x => x.Username == message.Username);

        await _serviceBusMessageSender.Send(new UserMetadataMessage()
        {
            Username = message.Username,
            MetadataPropertyName = "ProfileId",
            MetadataPropertyValue = $"{profile.ProfileId}"
        });
    }

}


