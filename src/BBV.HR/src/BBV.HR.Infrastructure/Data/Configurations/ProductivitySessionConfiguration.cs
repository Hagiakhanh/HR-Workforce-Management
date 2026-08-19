using BBV.HR.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BBV.HR.Infrastructure.Data.Configurations;

public class ProductivitySessionConfiguration : IEntityTypeConfiguration<ProductivitySession>
{
    public void Configure(EntityTypeBuilder<ProductivitySession> builder)
    {
        builder.ToTable("productivity_sessions");
        builder.HasKey(ps => ps.Id);
        builder.Property(ps => ps.StartedAt).IsRequired();

        builder.HasOne(ps => ps.Employee)
            .WithMany(e => e.ProductivitySessions)
            .HasForeignKey(ps => ps.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ps => ps.Project)
            .WithMany(p => p.ProductivitySessions)
            .HasForeignKey(ps => ps.ProjectId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
