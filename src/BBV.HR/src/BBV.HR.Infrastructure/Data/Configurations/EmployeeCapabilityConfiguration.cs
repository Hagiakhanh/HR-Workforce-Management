using BBV.HR.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BBV.HR.Infrastructure.Data.Configurations;

public class EmployeeCapabilityConfiguration : IEntityTypeConfiguration<EmployeeCapability>
{
    public void Configure(EntityTypeBuilder<EmployeeCapability> builder)
    {
        builder.ToTable("employee_capabilities");
        builder.HasKey(ec => ec.Id);
        builder.HasIndex(ec => new { ec.EmployeeId, ec.CapabilityId }).IsUnique();

        builder.HasOne(ec => ec.Employee)
            .WithMany(e => e.EmployeeCapabilities)
            .HasForeignKey(ec => ec.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ec => ec.Capability)
            .WithMany(c => c.EmployeeCapabilities)
            .HasForeignKey(ec => ec.CapabilityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
