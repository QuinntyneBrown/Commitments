using Innofactor.EfCoreJsonValueConverter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DashboardService.Infrastructure.Data.EntityConfigurations;

public class DashboardCardConfiguration : IEntityTypeConfiguration<DashboardCard>
{
    public void Configure(EntityTypeBuilder<DashboardCard> builder)
    {
        builder.Property(e => e.Options).HasJsonValueConversion();
    }
}
