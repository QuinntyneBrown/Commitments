using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Identity.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
{
    public IdentityDbContext CreateDbContext(string[] args)
    {
        var basePath = Path.GetFullPath("../../Commitments.Api");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<IdentityDbContext>();

        var connectionString = configuration.GetConnectionString("IdentityDb")
            ?? configuration.GetConnectionString("DefaultConnection");

        builder.UseSqlServer(connectionString);

        return new IdentityDbContext(builder.Options);
    }
}
