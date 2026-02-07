using Commitments.Shared;
using DigitalAssets.Core;
using DigitalAssets.Infrastructure.Data;
using FluentValidation;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddSqlServerDbContext<DigitalAssetsDbContext>("DigitalAssetsDb");

builder.AddRedisClient("redis");

builder.Services.AddScoped<IDigitalAssetsDbContext, DigitalAssetsDbContext>();

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
        Title = "DigitalAssets API",
        Description = "DigitalAssets Microservice API"
    });
    options.EnableAnnotations();
});

builder.Services.AddCors(options => options.AddPolicy(Constants.CorsPolicy,
    b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseSwagger(options => options.SerializeAsV2 = true);
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "DigitalAssets");
    options.RoutePrefix = string.Empty;
    options.DisplayOperationId();
});

app.UseCors(Constants.CorsPolicy);
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program { }
