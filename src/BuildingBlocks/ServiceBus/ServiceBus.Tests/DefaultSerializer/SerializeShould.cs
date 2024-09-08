// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Xunit;
using ServiceBus.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceBus.Tests.DefaultSerializer;

using DefaultSerializer = ServiceBus.Serialization.DefaultSerializer;


[Serializable()]
public class Message
{
    public Message()
    {
        

    }


    public string Value { get; set; }
}
public class SerializeShould
{
    [Fact]
    public void InsertTheExpectedBitsIntoBuffer()
    {
        // ARRANGE

        var services = new ServiceCollection().AddLogging().BuildServiceProvider();

        var message = new Message() { Value = "hi" };

        var sut = ActivatorUtilities.CreateInstance<DefaultSerializer>(services);

        // ACT

        var buffer = new byte[14];

        sut.Serialize(message, ref buffer);

        var copy = (Message)sut.Deserialize(buffer);


        // ASSERT

        Assert.Equal(message, copy);

    }

}

