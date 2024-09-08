// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;

namespace ServiceBus.Transport;

public class ServiceBusClientFactory : IServiceBusClientFactory
{
    private readonly ILogger<ServiceBusClientFactory> _logger;
    private readonly ILoggerFactory _loggerFactory;

    public static readonly string MultiCastGroupIp = "224.0.0.1";
    public const int BroadcastPort = 4681;

    public ServiceBusClientFactory(ILogger<ServiceBusClientFactory> logger, ILoggerFactory loggerFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
    }

    public IServiceBusClient Create(string topic)
    {
        _logger.LogInformation("Create");

        ServiceBusClient udpClient = default!;

        int i = 1;

        while (udpClient?.Client?.IsBound == null || udpClient.Client.IsBound == false)
        {
            try
            {
                udpClient = new ServiceBusClient(_loggerFactory.CreateLogger<ServiceBusClient>(), topic);

                udpClient.Client.Bind(IPEndPoint.Parse($"127.0.0.{i}:{BroadcastPort}"));

                udpClient.JoinMulticastGroup(IPAddress.Parse(MultiCastGroupIp), IPAddress.Parse($"127.0.0.{i}"));
            }
            catch (SocketException)
            {
                i++;
            }
        }

        return udpClient;
    }
}