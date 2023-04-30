// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Net;

namespace SeedData;

public class UdpClientFactory
{
    public static readonly string MultiCastGroupIp = "224.0.0.1";
    public const int BroadcastPort = 4681;
    public UdpClient Create()
    {
        UdpClient udpClient = null!;

        int i = 1;

        while (udpClient?.Client?.IsBound == null || udpClient.Client.IsBound == false)
        {
            try
            {
                udpClient = new UdpClient();

                udpClient.Client.Bind(IPEndPoint.Parse($"127.0.0.{i}:{BroadcastPort}"));

                udpClient.JoinMulticastGroup(IPAddress.Parse(MultiCastGroupIp), IPAddress.Parse($"127.0.0.{i}"));
            }
            catch (SocketException)
            {
                i++;
            }
        }

        return udpClient;
    }
}