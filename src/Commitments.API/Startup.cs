using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Commitments.Api.Behaviors;
using Commitments.Api.Hubs;
using Commitments.Core.Behaviours;
using Commitments.Core.Identity;
using Commitments.Core.Extensions;
using Commitments.Infrastructure.Extensions;
using static System.Convert;

namespace Commitments.Api
{
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
            services.AddDataStore(Configuration["Data:DefaultConnection:ConnectionString"]);
            services.AddMediatR(typeof(Startup));                        
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(EntityChangedBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
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
            app.UseSignalR(routes => routes.MapHub<AppHub>("/hub"));
            app.UseSwagger();
            app.UseSwaggerUI(options 
                => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Commitments API"));
        }
    }
}
