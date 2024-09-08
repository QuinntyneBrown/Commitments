// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace ServiceBus.Serialization;

public interface IMessageSerializer
{
    void Serialize(object message, ref byte[] buffer);

    object Deserialize(byte[] buffer);

}

