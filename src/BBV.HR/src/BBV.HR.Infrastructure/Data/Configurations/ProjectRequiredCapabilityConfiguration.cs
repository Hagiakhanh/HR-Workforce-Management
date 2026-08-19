using BBV.HR.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BBV.HR.Infrastructure.Data.Configurations;

public class ProjectRequiredCapabilityConfiguration : IEntityTypeConfiguration<ProjectRequiredCapability>
{
    public void Configure(EntityTypeBuilder<ProjectRequiredCapability> builder)
    {
        builder.ToTable("project_required_capabilities");
        builder.HasKey(prc => prc.Id);
        builder.HasIndex(prc => new { prc.ProjectId, prc.CapabilityId }).IsUnique();

        builder.HasOne(prc => prc.Project)
            .WithMany(p => p.ProjectRequiredCapabilities)
            .HasForeignKey(prc => prc.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(prc => prc.Capability)
            .WithMany(c => c.ProjectRequiredCapabilities)
            .HasForeignKey(prc => prc.CapabilityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
