// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Hubs;
using Commitments.Core.Misc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Commitments.Core;

public static class ConfigureServices
{
    public static void AddCoreServices(this IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
    {
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblyContaining<ICommitmentsClient>());

        var settings = new JsonSerializerSettings
        {
            ContractResolver = new SignalRContractResolver()
        };

        var serializer = JsonSerializer.Create(settings);

        services.Add(new ServiceDescriptor(typeof(JsonSerializer),
                                           provider => serializer,
                                           ServiceLifetime.Transient));
        services.AddSignalR();

        services.AddSecurity(environment, configuration);

        services.AddValidation(typeof(ICommitmentsClient));

        services.AddMessagingUdpServices();

        services.AddTelemetryServices();
    }

}

