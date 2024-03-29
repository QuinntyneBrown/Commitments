// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Messaging;
using Messaging.Udp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;

namespace IdentityService.Core;

public class ServiceBusMessageConsumer : BackgroundService
{
    private readonly ILogger<ServiceBusMessageConsumer> _logger;

    private readonly IServiceScopeFactory _serviceScopeFactory;

    private readonly IUdpClientFactory _udpClientFactory;

    private readonly string[] _supportedMessageTypes = new string[] { "UserCreateMessage" };

    public ServiceBusMessageConsumer(ILogger<ServiceBusMessageConsumer> logger, IServiceScopeFactory serviceScopeFactory, IUdpClientFactory udpClientFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        _udpClientFactory = udpClientFactory ?? throw new ArgumentNullException(nameof(udpClientFactory));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var client = _udpClientFactory.Create();

        while (!cancellationToken.IsCancellationRequested)
        {

            var result = await client.ReceiveAsync(cancellationToken);

            var json = Encoding.UTF8.GetString(result.Buffer);

            var message = System.Text.Json.JsonSerializer.Deserialize<ServiceBusMessage>(json)!;

            var messageType = message.MessageAttributes["MessageType"];

            if (_supportedMessageTypes.Contains(messageType))
            {
                var type = Type.GetType($"IdentityService.Core.Messages.{messageType}");

                var request = System.Text.Json.JsonSerializer.Deserialize(message.Body, type!)!;

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var sender = scope.ServiceProvider.GetRequiredService<ISender>();

                    await sender.Send(request, cancellationToken);
                }
            }

            await Task.Delay(0);
        }
    }

}


