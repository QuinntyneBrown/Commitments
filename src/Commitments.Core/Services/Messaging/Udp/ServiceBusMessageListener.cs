// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Services.Messaging.Internals;
using MessagePack;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;

namespace Commitments.Core.Services.Messaging.Udp;

public class ServiceBusMessageListener : Observable<IServiceBusMessage>, IServiceBusMessageListener
{
    private readonly ILogger<ServiceBusMessageListener> _logger;
    private readonly UdpClient _client;

    public ServiceBusMessageListener(
        ILogger<ServiceBusMessageListener> logger,
        IUdpClientFactory udpClientFactory
        )
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(udpClientFactory);

        _logger = logger;
        _client = udpClientFactory.Create();
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var result = await _client.ReceiveAsync(cancellationToken);

            var serviceBusMessage = MessagePackSerializer.Deserialize<ServiceBusMessage>(result.Buffer);

            Broadcast(serviceBusMessage);

            await Task.Delay(300);
        }
    }
}
