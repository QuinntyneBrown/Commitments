// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace ServiceBus.Transport;

public class ServiceBusMessage : IServiceBusMessage
{
    private readonly object _body;

    public ServiceBusMessage(byte[] body)
    {

    }

    public ServiceBusMessage(object body)
    {

    }

    public ServiceBusMessageHeader Header { get; set; }
    public byte[] Body { get; set; }

    public void Serialize(ref byte[] buffer)
    {

    }
}