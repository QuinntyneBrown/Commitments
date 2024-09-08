// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using ServiceBus.Transport;

namespace ServiceBus.Serialization;

public interface IMessageSerializerResolver
{
    Task<IMessageSerializer> Resolve(ServiceBusMessageHeader header);

}

