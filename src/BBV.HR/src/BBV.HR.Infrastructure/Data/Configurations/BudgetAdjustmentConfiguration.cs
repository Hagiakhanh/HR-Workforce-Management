using BBV.HR.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BBV.HR.Infrastructure.Data.Configurations;

public class BudgetAdjustmentConfiguration : IEntityTypeConfiguration<BudgetAdjustment>
{
    public void Configure(EntityTypeBuilder<BudgetAdjustment> builder)
    {
        builder.ToTable("budget_adjustments");
        builder.HasKey(ba => ba.Id);
        builder.Property(ba => ba.AdjustmentType).IsRequired();
        builder.Property(ba => ba.Amount).IsRequired();
        builder.Property(ba => ba.Status).IsRequired();

        builder.HasOne(ba => ba.Project)
            .WithMany(p => p.BudgetAdjustments)
            .HasForeignKey(ba => ba.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ba => ba.Requester)
            .WithMany(e => e.RequestedBudgetAdjustments)
            .HasForeignKey(ba => ba.RequestedBy)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(ba => ba.Approver)
            .WithMany(e => e.ApprovedBudgetAdjustments)
            .HasForeignKey(ba => ba.ApprovedBy)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
