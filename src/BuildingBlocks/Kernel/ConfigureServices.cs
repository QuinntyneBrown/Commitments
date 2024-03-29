// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Kernel;
using Newtonsoft.Json;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static void AddKernelServices(this IServiceCollection services)
    {

        var settings = new JsonSerializerSettings
        {
            ContractResolver = new HubCamelCasePropertyNamesContractResolver()
        };

        var serializer = JsonSerializer.Create(settings);

        services.Add(new ServiceDescriptor(typeof(JsonSerializer),
                                           provider => serializer,
                                           ServiceLifetime.Transient));
    }

}