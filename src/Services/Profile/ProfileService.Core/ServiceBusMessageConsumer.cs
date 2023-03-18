// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Messaging;
using Messaging.Udp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProfileService.Core.Messages;
using System.Text;

namespace ProfileService.Core;

public class ServiceBusMessageConsumer: BackgroundService
{
    private readonly ILogger<ServiceBusMessageConsumer> _logger;
    private readonly IUdpClientFactory _udpClientFactory;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    private readonly string[] _supportedMessageTypes = new string[] { 
        nameof(UserCreatedMessage) 
    };

    public ServiceBusMessageConsumer(
        ILogger<ServiceBusMessageConsumer> logger,
        IUdpClientFactory udpClientFactory,
        IServiceScopeFactory serviceScopeFactory
        ){
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _udpClientFactory = udpClientFactory ?? throw new ArgumentNullException(nameof(udpClientFactory));
        _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var client = _udpClientFactory.Create();

        while(!stoppingToken.IsCancellationRequested) {

            var result = await client.ReceiveAsync(stoppingToken);

            var json = Encoding.UTF8.GetString(result.Buffer);

            var message = System.Text.Json.JsonSerializer.Deserialize<ServiceBusMessage>(json)!;

            var messageType = message.MessageAttributes["MessageType"];

            if(_supportedMessageTypes.Contains(messageType))
            {
                var type = Type.GetType($"ProfileService.Core.Messages.{messageType}");

                var request = System.Text.Json.JsonSerializer.Deserialize(message.Body, type!)!;

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    await mediator.Send(request, stoppingToken);
                }
            }

            await Task.Delay(0);
        }
    }

}


