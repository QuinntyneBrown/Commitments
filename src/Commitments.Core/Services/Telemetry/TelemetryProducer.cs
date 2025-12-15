// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Services.Messaging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Commitments.Core.Services.Telemetry;

public class TelemetryProducer : BackgroundService
{
    private readonly ILogger<TelemetryProducer> _logger;
    private readonly IServiceBusMessageSender _serviceBusMessageSender;
    private readonly PeriodicTimer _periodicTimer = new PeriodicTimer(TimeSpan.FromMilliseconds(300));

    public TelemetryProducer(
        ILogger<TelemetryProducer> logger,
        IServiceBusMessageSender serviceBusMessageSender)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceBusMessageSender = serviceBusMessageSender ?? throw new ArgumentNullException(nameof(serviceBusMessageSender));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await _periodicTimer.WaitForNextTickAsync(stoppingToken))
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            await _serviceBusMessageSender.Send(new TelemetryMessage());
        }
    }
}
