using IdentityService.Core.Messages;

var client = new UdpClientFactory().Create();

await Send(client, new UserCreateMessage("seed", "seed"));

async Task Send(UdpClient client, object message)
{
    var messageType = message.GetType().Name;

    var serviceBusMessage = new ServiceBusMessage(new Dictionary<string, string>()
        {
            { "MessageType", messageType }

        }, JsonSerializer.Serialize(message));

    var json = JsonSerializer.Serialize(serviceBusMessage);

    var bytesToSend = System.Text.Encoding.UTF8.GetBytes(json);

    await client.SendAsync(bytesToSend, bytesToSend.Length, UdpClientFactory.MultiCastGroupIp, UdpClientFactory.BroadcastPort);
}