using BBV.HR.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace BBV.HR.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Ensure database is created
        await context.Database.MigrateAsync();

        // 1. Seed Employees
        if (!await context.Employees.AnyAsync())
        {
            var emp1 = new Employee
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                FirstName = "An",
                LastName = "Nguyen Van",
                EmployeeCode = "EMP-001"
            };
            var emp2 = new Employee
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                FirstName = "Binh",
                LastName = "Tran Thi",
                EmployeeCode = "EMP-002"
            };
            var emp3 = new Employee
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                FirstName = "Cuong",
                LastName = "Le Van",
                EmployeeCode = "EMP-003"
            };

            await context.Employees.AddRangeAsync(emp1, emp2, emp3);
            await context.SaveChangesAsync();
        }

        // 2. Seed Capabilities
        if (!await context.Capabilities.AnyAsync())
        {
            var cap1 = new Capability
            {
                Id = Guid.NewGuid(),
                Name = "Backend .NET Core",
                Category = "Engineering",
                Description = "C#, ASP.NET Core Web API, Entity Framework Core",
                CreatedAt = DateTime.UtcNow
            };
            var cap2 = new Capability
            {
                Id = Guid.NewGuid(),
                Name = "Database PostgreSQL",
                Category = "Engineering",
                Description = "PostgreSQL DB design, performance tuning & indexing",
                CreatedAt = DateTime.UtcNow
            };

            await context.Capabilities.AddRangeAsync(cap1, cap2);
            await context.SaveChangesAsync();
        }

        // 3. Seed Projects
        if (!await context.Projects.AnyAsync())
        {
            var managerId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var creatorId = Guid.Parse("11111111-1111-1111-1111-111111111111");

            var prj1 = new Project
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Code = "PRJ-HR",
                Name = "BBV HR Portal",
                Description = "Human Resource Management System for BBV",
                Status = "Active",
                ManagerId = managerId,
                CreatedBy = creatorId,
                StartDate = new DateOnly(2026, 1, 1),
                EndDate = new DateOnly(2026, 12, 31),
                TotalBudget = 500000000m,
                LaborBudget = 350000000m,
                BudgetWarningThreshold = 80,
                ExpectedTeamSizeFte = 5,
                DefaultWeeklyCapacityH = 40,
                Client = "Internal BBV",
                Priority = "High",
                TechStack = ".NET 10, PostgreSQL, React",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var prj2 = new Project
            {
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                Code = "PRJ-MOBILE",
                Name = "Employee Mobile App",
                Description = "Self-service mobile application for employees",
                Status = "Planning",
                ManagerId = managerId,
                CreatedBy = creatorId,
                StartDate = new DateOnly(2026, 6, 1),
                EndDate = new DateOnly(2026, 11, 30),
                TotalBudget = 300000000m,
                LaborBudget = 200000000m,
                BudgetWarningThreshold = 85,
                ExpectedTeamSizeFte = 3,
                DefaultWeeklyCapacityH = 40,
                Client = "Internal BBV",
                Priority = "Medium",
                TechStack = "Flutter, .NET 10 API",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await context.Projects.AddRangeAsync(prj1, prj2);
            await context.SaveChangesAsync();
        }

        // 4. Seed Project Members
        if (!await context.ProjectMembers.AnyAsync())
        {
            var prj1Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
            var emp1Id = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var emp2Id = Guid.Parse("22222222-2222-2222-2222-222222222222");

            var member1 = new ProjectMember
            {
                Id = Guid.NewGuid(),
                ProjectId = prj1Id,
                EmployeeId = emp1Id,
                ProjectRole = "Project Manager / Technical Lead",
                AllocationPct = 100,
                Status = "Active",
                StartDate = new DateOnly(2026, 1, 1),
                JoinedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var member2 = new ProjectMember
            {
                Id = Guid.NewGuid(),
                ProjectId = prj1Id,
                EmployeeId = emp2Id,
                ProjectRole = "Senior Backend Developer",
                AllocationPct = 100,
                Status = "Active",
                StartDate = new DateOnly(2026, 1, 15),
                JoinedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await context.ProjectMembers.AddRangeAsync(member1, member2);
            await context.SaveChangesAsync();
        }

        // 5. Seed Timesheets & TimeEntries for Effort Testing
        if (!await context.Timesheets.AnyAsync())
        {
            var emp2Id = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var prj1Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

            var timesheet = new Timesheet
            {
                Id = Guid.NewGuid(),
                EmployeeId = emp2Id,
                WeekStartDate = new DateOnly(2026, 8, 10),
                Status = "Approved",
                SubmittedAt = DateTime.UtcNow.AddDays(-2),
                ReviewedBy = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                ReviewedAt = DateTime.UtcNow.AddDays(-1),
                ReviewerNotes = "Looks good!"
            };

            var entry1 = new TimeEntry
            {
                Id = Guid.NewGuid(),
                TimesheetId = timesheet.Id,
                ProjectId = prj1Id,
                WorkDate = new DateOnly(2026, 8, 10),
                TimeType = "Regular",
                StartTime = new TimeOnly(8, 30),
                EndTime = new TimeOnly(17, 30),
                Notes = "Implemented Project Management APIs & Repositories"
            };

            var entry2 = new TimeEntry
            {
                Id = Guid.NewGuid(),
                TimesheetId = timesheet.Id,
                ProjectId = prj1Id,
                WorkDate = new DateOnly(2026, 8, 11),
                TimeType = "Regular",
                StartTime = new TimeOnly(8, 30),
                EndTime = new TimeOnly(17, 30),
                Notes = "Added EF Core PostgreSQL migration and unit tests"
            };

            timesheet.TimeEntries.Add(entry1);
            timesheet.TimeEntries.Add(entry2);

            await context.Timesheets.AddAsync(timesheet);
            await context.SaveChangesAsync();
        }
    }
}
