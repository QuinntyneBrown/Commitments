// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Services.Messaging.Udp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using MediatR;
using System.Linq;
using Commitments.Core.Services.Messaging;

namespace Commitments.Core;

public class ServiceBusMessageConsumer : BackgroundService
{
    private readonly ILogger<ServiceBusMessageConsumer> _logger;

    private readonly IServiceScopeFactory _serviceScopeFactory;

    private readonly IUdpClientFactory _udpClientFactory;

    private readonly string[] _supportedMessageTypes = new string[] { };

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
                _logger.LogInformation("Handling {messageType}", messageType);

                var type = Type.GetType($"Commitments.Core.Messages.{messageType}");

                var request = System.Text.Json.JsonSerializer.Deserialize(message.Body, type!)!;

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    await mediator.Send(request, cancellationToken);
                }
            }

            await Task.Delay(0);
        }
    }
}
