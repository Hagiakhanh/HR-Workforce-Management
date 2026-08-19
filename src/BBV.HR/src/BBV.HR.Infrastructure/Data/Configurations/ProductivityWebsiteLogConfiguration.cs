using BBV.HR.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BBV.HR.Infrastructure.Data.Configurations;

public class ProductivityWebsiteLogConfiguration : IEntityTypeConfiguration<ProductivityWebsiteLog>
{
    public void Configure(EntityTypeBuilder<ProductivityWebsiteLog> builder)
    {
        builder.ToTable("productivity_website_logs");
        builder.HasKey(pwl => pwl.Id);

        builder.HasOne(pwl => pwl.Session)
            .WithMany(ps => ps.WebsiteLogs)
            .HasForeignKey(pwl => pwl.SessionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
