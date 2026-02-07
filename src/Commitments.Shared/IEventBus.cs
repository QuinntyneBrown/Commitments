// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Commitments.Shared;

public interface IEventBus
{
    Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : IIntegrationEvent;
    Task SubscribeAsync<T>(Func<T, Task> handler, CancellationToken cancellationToken = default) where T : IIntegrationEvent;
}
