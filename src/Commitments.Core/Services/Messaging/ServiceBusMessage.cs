// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MessagePack;

namespace Commitments.Core.Services.Messaging;

[MessagePackObject]
public class ServiceBusMessage : IServiceBusMessage
{
    public ServiceBusMessage()
    {
        
    }
    public ServiceBusMessage(IDictionary<string, string> messageAttributes, byte[] body)
    {
        MessageAttributes = messageAttributes;
        Body = body;
    }

    [Key(0)]
    public string Type { get; set; } = string.Empty;

    [Key(1)]
    public IDictionary<string, string> MessageAttributes { get; init; }

    [Key(2)]
    public byte[] Body { get; init; }
}
