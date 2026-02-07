// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Infrastructure.Data;
using Commitments.Shared;
using FluentValidation;
using MediatR;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
.Enrich.FromLogContext()
.WriteTo.Console()
.CreateBootstrapLogger();

try
{
    Log.Information("Starting web host");

    var builder = WebApplication.CreateBuilder(args);

    builder.AddServiceDefaults();

    builder.AddSqlServerDbContext<CommitmentsDbContext>("CommitmentsDb");

    builder.AddRedisClient("redis");

    builder.Services.AddScoped<ICommitmentsDbContext, CommitmentsDbContext>();

    builder.Services.AddSingleton<IEventBus, RedisEventBus>();

    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

    builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

    builder.Services.AddControllers(o =>
    {
        o.Filters.Add(typeof(HttpGlobalExceptionFilter));
    });

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
            Description = "Commitments Microservice API",
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

        var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    }).AddSwaggerGenNewtonsoftSupport();

    var corsSettings = builder.Configuration.GetSection(CorsSettings.Section).Get<CorsSettings>() ?? new CorsSettings();

    builder.Services.AddCors(options => options.AddPolicy(Constants.CorsPolicy,
        b => b
        .WithOrigins(corsSettings.Origins)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed(isOriginAllowed: _ => true)
        .AllowCredentials()));

    var app = builder.Build();

    app.MapDefaultEndpoints();

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

    var services = (IServiceScopeFactory)app.Services.GetRequiredService(typeof(IServiceScopeFactory));

    using (var scope = services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<CommitmentsDbContext>();

        if (args.Contains("ci"))
            args = new string[4] { "dropdb", "migratedb", "seeddb", "stop" };

        if (args.Contains("migratedb"))
        {
            context.Database.Migrate();
        }

        if (args.Contains("seeddb"))
        {
            context.Seed();
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
