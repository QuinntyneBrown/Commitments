// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace ServiceBus.Transport;

public interface IServiceBusMessage
{
    ServiceBusMessageHeader Header { get; set; }
    byte[] Body { get; set; }

    void Serialize(ref byte[] buffer);
}