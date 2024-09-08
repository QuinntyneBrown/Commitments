// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace ServiceBus.Transport;

public interface IServiceBusClient
{
    Task PublishAsync(ServiceBusMessage message);

    Task<ServiceBusMessage> SubscribAsync(CancellationToken token);

}

