// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Kernel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static void AddApiServices(this IServiceCollection services)
    {

        services.AddControllers(o => o.Filters.Add(typeof(HttpGlobalExceptionFilter)))
            .AddMvcOptions(options => options.Filters.Add(new AuthorizeFilter()));

        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
        });

        services.AddEndpointsApiExplorer();

        services.AddHttpContextAccessor();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Dashboard Service",
                Description = "Dashboard Service",
                TermsOfService = new Uri("https://example.com/terms"),
                Contact = new OpenApiContact
                {
                    Name = "Quinntyne Brown",
                    Email = "quinntynebrown@gmail.com"
                },
                License = new OpenApiLicense
                {
                    Name = "Use under MIT",
                    Url = new Uri("https://opensource.org/licenses/MIT"),
                }
            });

            options.EnableAnnotations();

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

        }).AddSwaggerGenNewtonsoftSupport();

        services.AddCors(options => options.AddPolicy(Constants.CorsPolicy,
            builder => builder
            .WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(isOriginAllowed: _ => true)
            .AllowCredentials()));
    }
}