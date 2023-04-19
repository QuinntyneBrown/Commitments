// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using DashboardService.Core.Messages;

namespace DashboardService.Core.MessageHandlers;

public class UserCreatedMessageHandler : IRequestHandler<UserCreatedMessage>
{
    private readonly ILogger<UserCreatedMessageHandler> _logger;
    private readonly IDashboardServiceDbContext _context;
    public UserCreatedMessageHandler(
        ILogger<UserCreatedMessageHandler> logger,
        IDashboardServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context;
    }

    public async Task Handle(UserCreatedMessage message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Message Handled: {message}", message);

        _context.Users.Add(new()
        {
            Username = message.Username,
            Dashboards = new List<Dashboard>
            {
                new Dashboard("Default")
                {

                }
            }
        });

        await _context.SaveChangesAsync(cancellationToken);
    }
}