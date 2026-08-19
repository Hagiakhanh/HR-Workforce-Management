using BBV.HR.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BBV.HR.Infrastructure.Data.Configurations;

public class TimesheetConfiguration : IEntityTypeConfiguration<Timesheet>
{
    public void Configure(EntityTypeBuilder<Timesheet> builder)
    {
        builder.ToTable("timesheets");
        builder.HasKey(t => t.Id);
        builder.HasIndex(t => new { t.EmployeeId, t.WeekStartDate }).IsUnique();
        builder.Property(t => t.WeekStartDate).IsRequired();
        builder.Property(t => t.Status).IsRequired();

        builder.HasOne(t => t.Employee)
            .WithMany(e => e.Timesheets)
            .HasForeignKey(t => t.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.Reviewer)
            .WithMany(e => e.ReviewedTimesheets)
            .HasForeignKey(t => t.ReviewedBy)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
