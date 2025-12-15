// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core;
using Commitments.Infrastructure.Data;
using Commitments.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Commitments.Core.Hubs;
using Kernel;

Log.Logger = new LoggerConfiguration()
.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
.Enrich.FromLogContext()
.WriteTo.Console()
.CreateBootstrapLogger();

try
{
    Log.Information("Starting web host");

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddCoreServices(builder.Environment, builder.Configuration);

    builder.Services.AddInfrastructureServices(builder.Configuration.GetConnectionString("DefaultConnection")!);

    builder.Services.AddApiServices();

    var app = builder.Build();

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

    app.MapHub<CommitmentsHub>("/hub");

    var services = (IServiceScopeFactory)app.Services.GetRequiredService(typeof(IServiceScopeFactory));

    using (var scope = services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<CommitmentsDbContext>();

        if (args.Contains("ci"))
            args = new string[4] { "dropdb", "migratedb", "seeddb", "stop" };

        if (args.Contains("dropdb"))
        {
            context.Database.ExecuteSql($"DROP TABLE Commitments.Achievements;");

            context.Database.ExecuteSql($"DROP TABLE Commitments.Activities;");

            context.Database.ExecuteSql($"DROP TABLE Commitments.Behaviours;");

            context.Database.ExecuteSql($"DROP TABLE Commitments.Commitments;");

            context.Database.ExecuteSql($"DROP TABLE Commitments.Frequencies;");

            context.Database.ExecuteSql($"DROP TABLE Commitments.BehaviourTypes;");

            context.Database.ExecuteSql($"DROP TABLE Commitments.FrequencyTypes;");

            context.Database.ExecuteSql($"DROP TABLE Commitments.Profiles;");

            context.Database.ExecuteSql($"DROP SCHEMA Commitments;");

            context.Database.ExecuteSql($"DELETE from __EFMigrationsHistory where MigrationId like '%_Commitments_%';");
        }

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