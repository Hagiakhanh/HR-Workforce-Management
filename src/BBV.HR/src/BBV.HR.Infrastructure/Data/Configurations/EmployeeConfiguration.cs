using BBV.HR.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BBV.HR.Infrastructure.Data.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("employees");
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.EmployeeCode).IsUnique();
        builder.HasIndex(e => e.Email).IsUnique();
        builder.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(e => e.LastName).IsRequired().HasMaxLength(100);
        builder.Property(e => e.EmployeeCode).IsRequired().HasMaxLength(50);
        builder.Property(e => e.Email).IsRequired().HasMaxLength(255);
        builder.Property(e => e.PasswordHash).IsRequired();
        builder.Property(e => e.Role).IsRequired().HasMaxLength(50);
    }
}
