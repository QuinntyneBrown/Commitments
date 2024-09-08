// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using ServiceBus.Serialization;
using ServiceBus.Transport;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static void AddServiceBusServices(this IServiceCollection services)
    {
        services.AddSingleton<IMessageSerializerResolver,MessageSerializerResolver>();
        services.AddSingleton<IMessageSerializer,DefaultSerializer>();
        services.AddSingleton<IServiceBusClientFactory, ServiceBusClientFactory>();
    }
}


