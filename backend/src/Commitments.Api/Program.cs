// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments;
using Commitments.Data;
using Commitments.Shared;
using Dashboard;
using Dashboard.Data;
using DigitalAssets;
using DigitalAssets.Data;
using FluentValidation;
using Identity;
using Identity.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using System.Reflection;

Log.Logger = new LoggerConfiguration()
.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
.Enrich.FromLogContext()
.WriteTo.Console()
.CreateBootstrapLogger();

try
{
    Log.Information("Starting web host");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console());

    var moduleAssemblies = new[]
    {
        typeof(Commitments.ModuleExtensions).Assembly,
        typeof(Identity.ModuleExtensions).Assembly,
        typeof(Dashboard.ModuleExtensions).Assembly,
        typeof(DigitalAssets.ModuleExtensions).Assembly
    };

    builder.Services.AddCommitmentsModule(builder.Configuration);
    builder.Services.AddIdentityModule(builder.Configuration);
    builder.Services.AddDashboardModule(builder.Configuration);
    builder.Services.AddDigitalAssetsModule(builder.Configuration);

    builder.Services.AddSingleton<IEventBus, InMemoryEventBus>();

    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(moduleAssemblies));

    foreach (var assembly in moduleAssemblies)
    {
        builder.Services.AddValidatorsFromAssembly(assembly);
    }

    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

    var mvcBuilder = builder.Services.AddControllers(o =>
    {
        o.Filters.Add(typeof(HttpGlobalExceptionFilter));
    }).AddNewtonsoftJson();

    foreach (var assembly in moduleAssemblies)
    {
        mvcBuilder.AddApplicationPart(assembly);
    }

    builder.Services.AddApiVersioning(options =>
    {
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
    });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddHttpContextAccessor();

    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Version = "v1",
            Title = "Commitments",
            Description = "Commitments API",
            TermsOfService = new Uri("https://example.com/terms"),
            Contact = new Microsoft.OpenApi.Models.OpenApiContact
            {
                Name = "Quinntyne Brown",
                Email = "quinntynebrown@gmail.com"
            },
            License = new Microsoft.OpenApi.Models.OpenApiLicense
            {
                Name = "Use under MIT",
                Url = new Uri("https://opensource.org/licenses/MIT"),
            }
        });

        options.EnableAnnotations();

        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
        if (File.Exists(xmlPath))
        {
            options.IncludeXmlComments(xmlPath);
        }

    }).AddSwaggerGenNewtonsoftSupport();

    builder.Services.AddHealthChecks();

    var corsSettings = builder.Configuration.GetSection(CorsSettings.Section).Get<CorsSettings>() ?? new CorsSettings();

    builder.Services.AddCors(options => options.AddPolicy(Constants.CorsPolicy,
        b => b
        .WithOrigins(corsSettings.Origins)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed(isOriginAllowed: _ => true)
        .AllowCredentials()));

    var app = builder.Build();

    app.MapHealthChecks("/health");

    app.UseSwagger(options => options.SerializeAsV2 = true);

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Commitments");
        options.RoutePrefix = string.Empty;
        options.DisplayOperationId();
    });

    app.UseCors(Constants.CorsPolicy);

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    using (var scope = app.Services.CreateScope())
    {
        var commitmentsContext = scope.ServiceProvider.GetRequiredService<CommitmentsDbContext>();
        var identityContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
        var dashboardContext = scope.ServiceProvider.GetRequiredService<DashboardDbContext>();
        var digitalAssetsContext = scope.ServiceProvider.GetRequiredService<DigitalAssetsDbContext>();

        if (args.Contains("ci"))
            args = new[] { "dropdb", "migratedb", "seeddb", "stop" };

        if (args.Contains("migratedb"))
        {
            commitmentsContext.Database.Migrate();
            identityContext.Database.Migrate();
            dashboardContext.Database.Migrate();
            digitalAssetsContext.Database.Migrate();
        }

        if (args.Contains("seeddb"))
        {
            commitmentsContext.Seed();
        }

        if (args.Contains("stop"))
            Environment.Exit(0);
    }

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { }
