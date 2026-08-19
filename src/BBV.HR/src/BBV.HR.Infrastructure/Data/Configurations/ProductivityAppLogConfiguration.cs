using BBV.HR.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BBV.HR.Infrastructure.Data.Configurations;

public class ProductivityAppLogConfiguration : IEntityTypeConfiguration<ProductivityAppLog>
{
    public void Configure(EntityTypeBuilder<ProductivityAppLog> builder)
    {
        builder.ToTable("productivity_app_logs");
        builder.HasKey(pal => pal.Id);
        builder.Property(pal => pal.AppName).IsRequired();

        builder.HasOne(pal => pal.Session)
            .WithMany(ps => ps.AppLogs)
            .HasForeignKey(pal => pal.SessionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
