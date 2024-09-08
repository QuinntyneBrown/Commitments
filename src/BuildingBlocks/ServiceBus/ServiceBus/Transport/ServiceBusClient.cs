// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;
using System.Net.Sockets;

namespace ServiceBus.Transport;

public class ServiceBusClient : UdpClient, IServiceBusClient
{
    private readonly ILogger<ServiceBusClient> _logger;

    private readonly string _topic;

    public ServiceBusClient(ILogger<ServiceBusClient> logger, string topic)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _topic = topic;
    }

    public async Task PublishAsync(ServiceBusMessage message)
    {
        _logger.LogInformation("Publishing to Ip and Port: {multiCastGroupIp}:{broadcastPort}", ServiceBusClientFactory.MultiCastGroupIp, ServiceBusClientFactory.BroadcastPort);

        byte[] buffer = new byte[4096];

        message.Serialize(ref buffer);

        await SendAsync(buffer, buffer.Length, ServiceBusClientFactory.MultiCastGroupIp, ServiceBusClientFactory.BroadcastPort);

    }

    public async Task<ServiceBusMessage> SubscribAsync(CancellationToken token)
    {
        _logger.LogInformation("Subscribe to message.");

        do
        {
            var result = await ReceiveAsync(token);

            var message = new ServiceBusMessage(result.Buffer);

            if(message.Header.Topic != _topic)
            {
                continue;
            }

        }while(true);
    }

}

