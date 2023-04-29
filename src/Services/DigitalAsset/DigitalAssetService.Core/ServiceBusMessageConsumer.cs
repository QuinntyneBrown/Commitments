// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Messaging;
using Newtonsoft.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;
using Messaging.Udp;

namespace DigitalAssetService.Core;

public class ServiceBusMessageConsumer : BackgroundService
{
    private readonly ILogger<ServiceBusMessageConsumer> _logger;

    private readonly IMediator _mediator;

    private readonly IUdpClientFactory _udpClientFactory;

    private readonly string[] _supportedMessageTypes = new string[] { };

    public ServiceBusMessageConsumer(ILogger<ServiceBusMessageConsumer> logger, IMediator mediator, IUdpClientFactory udpClientFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _udpClientFactory = udpClientFactory ?? throw new ArgumentNullException(nameof(udpClientFactory));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var client = _udpClientFactory.Create();

        while (!stoppingToken.IsCancellationRequested)
        {

            var result = await client.ReceiveAsync(stoppingToken);

            var json = Encoding.UTF8.GetString(result.Buffer);

            var message = System.Text.Json.JsonSerializer.Deserialize<ServiceBusMessage>(json)!;

            var messageType = message.MessageAttributes["MessageType"];

            if (_supportedMessageTypes.Contains(messageType))
            {
                var type = Type.GetType($"DigitalAssetService.Core.Messages.{messageType}");

                var request = (IRequest)System.Text.Json.JsonSerializer.Deserialize(message.Body, type!)!;

                await _mediator.Send(request, stoppingToken);
            }

            await Task.Delay(300);
        }
    }

}