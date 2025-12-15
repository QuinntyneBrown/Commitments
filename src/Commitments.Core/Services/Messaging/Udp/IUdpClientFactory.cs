// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Net.Sockets;

namespace Commitments.Core.Services.Messaging.Udp;

public interface IUdpClientFactory
{
    UdpClient Create();
}
