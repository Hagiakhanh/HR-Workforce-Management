using BBV.HR.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BBV.HR.Infrastructure.Data.Configurations;

public class ProductivityScreenshotConfiguration : IEntityTypeConfiguration<ProductivityScreenshot>
{
    public void Configure(EntityTypeBuilder<ProductivityScreenshot> builder)
    {
        builder.ToTable("productivity_screenshots");
        builder.HasKey(psc => psc.Id);
        builder.Property(psc => psc.CapturedAt).IsRequired();
        builder.Property(psc => psc.FileUrl).IsRequired();

        builder.HasOne(psc => psc.Session)
            .WithMany(ps => ps.Screenshots)
            .HasForeignKey(psc => psc.SessionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
