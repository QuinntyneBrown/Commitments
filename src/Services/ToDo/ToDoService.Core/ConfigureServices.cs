// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using ToDoService.Core;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static void AddCoreServices(this IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
    {
        services.AddHostedService<ServiceBusMessageConsumer>();
        services.AddMessagingUdpServices();
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblyContaining<IToDoServiceDbContext>());
        services.AddSecurity(environment, configuration);
        services.AddValidation(typeof(IToDoServiceDbContext));
        services.AddTelemetryServices();
    }
}