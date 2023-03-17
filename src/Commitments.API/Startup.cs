// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Api.Behaviors;
using Commitments.Core;
using Commitments.Core.Behaviours;
using Commitments.Core.Extensions;
using Commitments.Core.Identity;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static System.Convert;


namespace Commitments.Api;

public class Startup
{
    public Startup(IConfiguration configuration)
        => Configuration = configuration;

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddCustomMvc();
        services.AddCustomSecurity(Configuration);
        services.AddCustomSignalR();                        
        services.AddCustomSwagger();            
        services.AddInfrastructureServices(Configuration["Data:DefaultConnection:ConnectionString"]);
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblyContaining<Startup>());                        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(EntityChangedBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ProfileChangedBehavior<,>));
    }

    public void Configure(IApplicationBuilder app)
    {
        if (ToBoolean(Configuration["isTest"]))
            app.UseMiddleware<AutoAuthenticationMiddleware>();

        app.UseAuthentication();            
        app.UseCors("CorsPolicy");            
        app.UseMvc();
        //app.MapHub
        //app.MapHub<AppHub>("/hub");
        app.UseSwagger();
        app.UseSwaggerUI(options 
            => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Commitments API"));
    }
}

