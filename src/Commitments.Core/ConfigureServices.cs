// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Hubs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Commitments.Core;

public static class ConfigureServices
{
    public static void AddCoreServices(this IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
    {
        services.AddHostedService<ServiceBusMessageConsumer>();

        // MediatR handlers moved to Api project

        //services.AddKernelServices();

        services.AddSignalR();

        //services.AddSecurity(environment, configuration);

        //services.AddValidation(typeof(ICommitmentsClient));

        //services.AddMessagingUdpServices();

        //services.AddTelemetryServices();
    }

}