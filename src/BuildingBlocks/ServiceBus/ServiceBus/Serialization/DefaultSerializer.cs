// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ServiceBus.Serialization;

public interface IMessage
{

}

public class DefaultSerializer : IMessageSerializer
{
    private readonly ILogger<DefaultSerializer> _logger;

    public DefaultSerializer(ILogger<DefaultSerializer> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public object Deserialize(byte[] buffer)
    {
        return JsonSerializer.Deserialize<object>(buffer);
    }

    public void Serialize(object message, ref byte[] buffer)
    {
        var source = JsonSerializer.SerializeToUtf8Bytes(message);

        System.Buffer.BlockCopy(source, 0, buffer, 0,source.Length);
    }
}

