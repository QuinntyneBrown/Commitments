using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Dashboard.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DashboardDbContext>
{
    public DashboardDbContext CreateDbContext(string[] args)
    {
        var basePath = Path.GetFullPath("../../Commitments.Api");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<DashboardDbContext>();

        var connectionString = configuration.GetConnectionString("DashboardDb")
            ?? configuration.GetConnectionString("DefaultConnection");

        builder.UseSqlServer(connectionString);

        return new DashboardDbContext(builder.Options);
    }
}
