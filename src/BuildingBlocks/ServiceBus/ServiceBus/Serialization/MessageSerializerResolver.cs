// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;
using ServiceBus.Transport;

namespace ServiceBus.Serialization;

public class MessageSerializerResolver : IMessageSerializerResolver
{
    private readonly ILogger<MessageSerializerResolver> _logger;

    public MessageSerializerResolver(ILogger<MessageSerializerResolver> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IMessageSerializer> Resolve(ServiceBusMessageHeader header)
    {
        throw new NotImplementedException();

    }

}

