// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using DigitalAssetService.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static void AddCoreServices(this IServiceCollection services, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
    {
        services.AddHostedService<ServiceBusMessageConsumer>();
        services.AddMessagingUdpServices();
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblyContaining<IDigitalAssetServiceDbContext>());
        services.AddSecurity(webHostEnvironment, configuration);
        services.AddValidation(typeof(IDigitalAssetServiceDbContext));
        services.AddTelemetryServices();
    }
}