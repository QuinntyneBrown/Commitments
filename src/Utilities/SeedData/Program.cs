var sender = new UdpClientFactory().Create();



async Task Send(UdpClient sender, object message)
{
    var messageType = message.GetType().Name;

    var serviceBusMessage = new ServiceBusMessage(new Dictionary<string, string>()
        {
            { "MessageType", messageType }

        }, JsonSerializer.Serialize(message));

    var json = JsonSerializer.Serialize(serviceBusMessage);

    var bytesToSend = System.Text.Encoding.UTF8.GetBytes(json);

    await sender.SendAsync(bytesToSend, bytesToSend.Length, UdpClientFactory.MultiCastGroupIp, UdpClientFactory.BroadcastPort);
}